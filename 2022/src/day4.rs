use crate::FromLines;

#[derive(Clone, Copy)]
struct Assingment {
    start: u32,
    end: u32,
}

impl Assingment {
    fn length(self) -> u32 {
        self.end - self.start + 1
    }
}

#[derive(Clone, Copy)]
struct Pair {
    first: Assingment,
    second: Assingment,
}

pub(crate) struct Solution {
    assignments: Vec<Pair>,
}

impl FromLines for Solution {
    fn from_lines<Iter: Iterator<Item = String>>(lines: Iter) -> Self {
        let pairs = lines
            .map(|line| {
                let assignments: Vec<Assingment> = line
                    .split(',')
                    .map(|interval| {
                        let numbers: Vec<&str> = interval.split('-').collect();
                        let start: u32 = numbers[0].parse().unwrap();
                        let end: u32 = numbers[1].parse().unwrap();
                        Assingment { start, end }
                    })
                    .collect();
                Pair {
                    first: assignments[0],
                    second: assignments[1],
                }
            })
            .collect();
        Solution { assignments: pairs }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 4;

    fn solve_first_part(&self) -> String {
        self.assignments
            .iter()
            .filter(|pair| {
                let (long, short) = if pair.first.length() > pair.second.length() {
                    (pair.first, pair.second)
                } else {
                    (pair.second, pair.first)
                };
                return long.start <= short.start && long.end >= short.end;
            })
            .count()
            .to_string()
    }

    fn solve_second_part(&self) -> String {
        self.assignments
            .iter()
            .filter(|pair| {
                let no_overlap =
                    pair.first.start > pair.second.end || pair.second.start > pair.first.end;
                return !no_overlap;
            })
            .count()
            .to_string()
    }
}
