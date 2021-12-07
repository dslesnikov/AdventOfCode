use std::{
    fs::File,
    io::{self, BufRead, Read},
};

pub trait FromLines {
    fn new(lines: Vec<String>) -> Self;
}

pub trait FromContent {
    fn new(content: String) -> Self;
}

pub trait CreateFromLines {
    fn create() -> Self;
}

pub trait CreateFromContent {
    fn create() -> Self;
}

pub struct InputParser;

impl InputParser {
    pub(crate) fn from_content<S: crate::Solution + FromContent>(&self) -> S {
        let content = self.get_content(S::DAY);
        <S as FromContent>::new(content)
    }

    pub(crate) fn from_lines<S: crate::Solution + FromLines>(&self) -> S {
        let lines = self.get_lines(S::DAY);
        <S as FromLines>::new(lines)
    }

    fn get_lines(&self, day: i32) -> Vec<String> {
        let file = self.open_file(day);
        let lines = io::BufReader::new(file).lines().map(|line| line.unwrap());
        lines.collect()
    }

    fn get_content(&self, day: i32) -> String {
        let mut content = String::new();
        let mut input_file = self.open_file(day);
        input_file.read_to_string(&mut content).unwrap();
        content
    }

    fn open_file(&self, day: i32) -> File {
        let file_name = format!("input/{}.txt", day);
        let input_file = File::open(file_name).unwrap();
        input_file
    }
}
