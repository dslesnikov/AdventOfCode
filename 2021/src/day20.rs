use crate::parsing::{FromLines, InputParser};

struct Image {
    data: Vec<Vec<bool>>,
    outside_value: bool,
}

pub struct Solution {
    algorithm: Vec<bool>,
    image: Image,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let algorithm = lines[0].chars().map(|ch| ch == '#').collect();
        let data = lines
            .iter()
            .skip(2)
            .map(|line| line.chars().map(|char| char == '#').collect())
            .collect();
        Solution {
            algorithm,
            image: Image {
                data,
                outside_value: false,
            },
        }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 20;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        self.enhance_image(2).to_string()
    }

    fn solve_second_part(&self) -> String {
        self.enhance_image(50).to_string()
    }
}

impl Solution {
    fn enhance_image(&self, steps: usize) -> usize {
        let initial_image = &self.image;
        let mut new_image: Image;
        let mut current_image = initial_image;
        for _ in 1..=steps {
            let new_width = current_image.data[0].len() as i32 + 2;
            let new_height = current_image.data.len() as i32 + 2;
            let mut new_data = vec![vec![false; new_width as usize]; new_height as usize];
            for y in 0..new_height {
                for x in 0..new_width {
                    if self.should_be_lit(current_image, x - 1, y - 1) {
                        new_data[y as usize][x as usize] = true;
                    }
                }
            }
            new_image = Image {
                data: new_data,
                outside_value: if current_image.outside_value {
                    self.algorithm[self.algorithm.len() - 1]
                } else {
                    self.algorithm[0]
                },
            };
            current_image = &new_image;
        }
        current_image
            .data
            .iter()
            .flatten()
            .filter(|&&item| item)
            .count()
    }

    fn should_be_lit(&self, image: &Image, x: i32, y: i32) -> bool {
        let neighbours = [
            (x + 1, y + 1),
            (x, y + 1),
            (x - 1, y + 1),
            (x + 1, y),
            (x, y),
            (x - 1, y),
            (x + 1, y - 1),
            (x, y - 1),
            (x - 1, y - 1),
        ];
        let mut algo_index: usize = 0;
        let height = image.data.len() as i32;
        let width = image.data[0].len() as i32;
        for (index, pair) in neighbours.into_iter().enumerate() {
            if pair.0 < 0 || pair.1 < 0 || pair.0 >= width || pair.1 >= height {
                algo_index |= (image.outside_value as usize) << index;
                continue;
            }
            let item = image.data[pair.1 as usize][pair.0 as usize] as usize;
            algo_index |= item << index;
        }
        return self.algorithm[algo_index];
    }
}
