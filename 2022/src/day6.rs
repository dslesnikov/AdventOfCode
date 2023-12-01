use crate::FromContent;

pub(crate) struct Solution {
    input: String,
}

impl FromContent for Solution {
    fn from_content(content: String) -> Self {
        Solution { input: content }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 6;

    fn solve_first_part(&self) -> String {
        self.find_first_unique_occurence(4).to_string()
    }

    fn solve_second_part(&self) -> String {
        self.find_first_unique_occurence(14).to_string()
    }
}

impl Solution {
    fn find_first_unique_occurence(&self, window_size: usize) -> usize {
        let length = self.input.len();
        let chars: Vec<char> = self.input.chars().collect();
        let mut visited: [u32; 26] = [0; 26];
        for i in 0..window_size {
            let shift = chars[i] as usize - 'a' as usize;
            visited[shift] += 1;
        }
        for i in window_size..length {
            let mut shift = chars[i - window_size] as usize - 'a' as usize;
            visited[shift] -= 1;
            shift = chars[i] as usize - 'a' as usize;
            visited[shift] += 1;
            let ones_count = visited.iter().filter(|&&item| item == 1).count();
            if ones_count == window_size {
                return i + 1;
            }
        }
        panic!("Wrong input")
    }
}
