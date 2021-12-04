trait Solution {
    type Output;
    fn solve_first_part(&self) -> Self::Output;
    fn solve_second_part(&self) -> Self::Output;
}

mod day4;

fn main() {
    let solution = day4::Solution::new();
    let first = solution.solve_first_part();
    println!("First part: {}", first);
    let second = solution.solve_second_part();
    println!("Second part: {}", second);
}
