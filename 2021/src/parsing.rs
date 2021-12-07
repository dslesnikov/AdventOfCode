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
    pub(crate) fn from_content<S: crate::Solution + FromContent>() -> S {
        let content = Self::get_content(S::DAY);
        <S as FromContent>::new(content)
    }

    pub(crate) fn from_lines<S: crate::Solution + FromLines>() -> S {
        let lines = Self::get_lines(S::DAY);
        <S as FromLines>::new(lines)
    }

    fn get_lines(day: i32) -> Vec<String> {
        let file = Self::open_file(day);
        let lines = io::BufReader::new(file).lines().map(|line| line.unwrap());
        lines.collect()
    }

    fn get_content(day: i32) -> String {
        let mut content = String::new();
        let mut input_file = Self::open_file(day);
        input_file.read_to_string(&mut content).unwrap();
        content
    }

    fn open_file(day: i32) -> File {
        let file_name = format!("input/{}.txt", day);
        let input_file = File::open(file_name).unwrap();
        input_file
    }
}
