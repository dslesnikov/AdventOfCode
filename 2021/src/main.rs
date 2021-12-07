trait Solution {
    type Output;
    fn solve_first_part(&self) -> Self::Output;
    fn solve_second_part(&self) -> Self::Output;
}

mod day7;

fn main() {
    let solution = day7::Solution::new();
    let first = solution.solve_first_part();
    println!("First part: {}", first);
    let second = solution.solve_second_part();
    println!("Second part: {}", second);
}
