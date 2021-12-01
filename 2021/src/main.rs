trait Solution {
    type Output;
    fn solve_first_part(&self) -> Self::Output;
    fn solve_second_part(&self) -> Self::Output;
}

mod day1;

fn main() {
    let day1 = day1::Solution::new();
    let solution = day1.solve_first_part();
    println!("First part: {}", solution);
    let solution = day1.solve_second_part();
    println!("Second part: {}", solution);
}
