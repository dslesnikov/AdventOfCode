use std::{cell::RefCell, collections::HashMap, rc::Rc, str::FromStr};

use crate::FromLines;

struct File {
    size: u32,
}

struct Directory {
    size: u32,
    files: Vec<Rc<RefCell<File>>>,
    directories: HashMap<String, Rc<RefCell<Directory>>>,
    parent: Option<Rc<RefCell<Directory>>>,
}

pub(crate) struct Solution {
    root: Rc<RefCell<Directory>>,
}

enum Mode {
    Command,
    Output,
}

enum Command {
    ChangeDirectory(String),
    List,
}

impl FromStr for Command {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        if s.starts_with("$ ls") {
            return Ok(Command::List);
        }
        let dir_name: String = s.chars().skip(5).collect();
        return Ok(Command::ChangeDirectory(dir_name));
    }
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let root_directory = Directory {
            files: vec![],
            directories: HashMap::new(),
            parent: None,
            size: 0,
        };
        let root = Rc::new(RefCell::new(root_directory));
        let mut current_dir = root.clone();
        for line in lines {
            let mode = if line.starts_with('$') {
                Mode::Command
            } else {
                Mode::Output
            };
            match mode {
                Mode::Command => {
                    let command: Command = line.parse().unwrap();
                    match command {
                        Command::List => {
                            continue;
                        }
                        Command::ChangeDirectory(dir_name) => {
                            if dir_name == "/" {
                                current_dir = root.clone();
                                continue;
                            }
                            if dir_name == ".." {
                                let parent =
                                    current_dir.borrow_mut().parent.as_mut().unwrap().clone();
                                current_dir = parent;
                                continue;
                            }
                            let child: Rc<RefCell<Directory>> = current_dir
                                .borrow_mut()
                                .directories
                                .get(&dir_name)
                                .unwrap()
                                .to_owned();
                            current_dir = child.clone();
                        }
                    }
                }
                Mode::Output => {
                    if line.starts_with("dir") {
                        let dir_name: String = line.chars().skip(4).collect();
                        let directory = Directory {
                            parent: Some(current_dir.clone()),
                            directories: HashMap::new(),
                            files: Vec::new(),
                            size: 0,
                        };
                        current_dir
                            .borrow_mut()
                            .directories
                            .insert(dir_name, Rc::new(RefCell::new(directory)));
                    }
                    if line.chars().nth(0).unwrap().is_numeric() {
                        let split: Vec<&str> = line.split(' ').collect();
                        let size: u32 = split[0].parse().unwrap();
                        let file = File { size };
                        current_dir
                            .borrow_mut()
                            .files
                            .push(Rc::new(RefCell::new(file)));
                        continue;
                    }
                }
            }
        }
        Solution { root }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 7;

    fn solve_first_part(&self) -> String {
        self.fill_sizes();
        let result = Self::sum_small_directories(self.root.clone());
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let total = 70000000;
        let need_to_have = 30000000;
        let occupied = self.root.borrow().size;
        let free = total - occupied;
        let space_to_free = need_to_have - free;
        let result = Self::find_smallest_dir_larger_than_size(self.root.clone(), space_to_free);
        return result.to_string();
    }
}

impl Solution {
    fn find_smallest_dir_larger_than_size(dir: Rc<RefCell<Directory>>, size: u32) -> u32 {
        let children_resut = dir
            .borrow()
            .directories
            .values()
            .filter(|child| child.borrow().size > size)
            .map(|child| Self::find_smallest_dir_larger_than_size(child.clone(), size))
            .min();
        let current_size = dir.borrow().size;
        if let Some(ch_size) = children_resut {
            if current_size > size && current_size < ch_size {
                return current_size;
            }
            return ch_size;
        }
        if current_size > size {
            return current_size;
        }
        return u32::MAX;
    }

    fn sum_small_directories(dir: Rc<RefCell<Directory>>) -> u32 {
        let large_children: u32 = dir
            .borrow()
            .directories
            .values()
            .map(|child| Self::sum_small_directories(child.clone()))
            .sum();
        if dir.borrow().size <= 100000 {
            return large_children + dir.borrow().size;
        }
        return large_children;
    }

    fn fill_sizes(&self) {
        Self::fill_directory_size(self.root.clone());
    }

    fn fill_directory_size(dir: Rc<RefCell<Directory>>) -> u32 {
        let child_sizes: u32 = dir
            .borrow_mut()
            .directories
            .values()
            .map(|child| Self::fill_directory_size(child.clone()))
            .sum();
        let file_sizes: u32 = dir
            .borrow()
            .files
            .iter()
            .map(|file| file.borrow().size)
            .sum();
        dir.borrow_mut().size = file_sizes + child_sizes;
        return dir.borrow().size;
    }
}
