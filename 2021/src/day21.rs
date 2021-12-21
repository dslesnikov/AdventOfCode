use regex::Regex;

use crate::parsing::{FromContent, InputParser};

pub struct Solution {
    player_one: u8,
    player_two: u8,
}

impl FromContent for Solution {
    fn new(content: String) -> Self {
        let regex = Regex::new(r"Player \d starting position: (\d)").unwrap();
        let captures = regex.captures_iter(&content);
        let mut positions: [u8; 2] = [0, 0];
        for (index, capture) in captures.enumerate() {
            let position_match = capture.get(1).unwrap();
            positions[index] = position_match.as_str().parse().unwrap();
        }
        Solution {
            player_one: positions[0],
            player_two: positions[1],
        }
    }
}

#[derive(Clone, Copy)]
struct PlayerState {
    score: usize,
    position: u8,
}

impl PlayerState {
    fn new(position: u8) -> PlayerState {
        PlayerState { score: 0, position }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 21;

    fn create() -> Self {
        InputParser::from_content()
    }

    fn solve_first_part(&self) -> String {
        let mut players = [
            PlayerState::new(self.player_one - 1),
            PlayerState::new(self.player_two - 1),
        ];
        let mut current_player_index = 0;
        let mut roll_count: usize = 0;
        let final_result: u64;
        loop {
            let steps = (roll_count + 2) * 3;
            roll_count += 3;
            players[current_player_index] =
                Solution::make_steps(players[current_player_index], steps);
            let another_player_index = 1 - current_player_index;
            if players[current_player_index].score >= 1000 {
                let another_score = players[another_player_index].score;
                final_result = another_score as u64 * roll_count as u64;
                break;
            }
            current_player_index = another_player_index;
        }
        final_result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let initial_states = [
            PlayerState::new(self.player_one - 1),
            PlayerState::new(self.player_two - 1),
        ];
        let result = Self::simulate_quantum_game(initial_states, 0, (0, 0));
        result.0.max(result.1).to_string()
    }
}

fn add(left: (u64, u64), right: (u64, u64)) -> (u64, u64) {
    (left.0 + right.0, left.1 + right.1)
}

fn mul(k: u64, pair: (u64, u64)) -> (u64, u64) {
    (pair.0 * k, pair.1 * k)
}

impl Solution {
    fn make_steps(player: PlayerState, steps: usize) -> PlayerState {
        let new_position = ((player.position as usize + steps) % 10) as u8;
        PlayerState {
            position: new_position,
            score: player.score + (new_position + 1) as usize,
        }
    }

    fn simulate_quantum_game(
        player_states: [PlayerState; 2],
        current_player: usize,
        previous: (u64, u64),
    ) -> (u64, u64) {
        if player_states[0].score >= 21 {
            return (previous.0 + 1, previous.1);
        }
        if player_states[1].score >= 21 {
            return (previous.0, previous.1 + 1);
        }
        let mut result = Self::make_move(player_states, current_player, 3, previous);
        result = add(
            result,
            mul(
                3,
                Self::make_move(player_states, current_player, 4, previous),
            ),
        );
        result = add(
            result,
            mul(
                6,
                Self::make_move(player_states, current_player, 5, previous),
            ),
        );
        result = add(
            result,
            mul(
                7,
                Self::make_move(player_states, current_player, 6, previous),
            ),
        );
        result = add(
            result,
            mul(
                6,
                Self::make_move(player_states, current_player, 7, previous),
            ),
        );
        result = add(
            result,
            mul(
                3,
                Self::make_move(player_states, current_player, 8, previous),
            ),
        );
        result = add(
            result,
            Self::make_move(player_states, current_player, 9, previous),
        );
        result
    }

    fn make_move(
        mut player_states: [PlayerState; 2],
        current_player: usize,
        rolled: usize,
        previous: (u64, u64),
    ) -> (u64, u64) {
        player_states[current_player] = Self::make_steps(player_states[current_player], rolled);
        let results = Self::simulate_quantum_game(player_states, 1 - current_player, previous);
        add(previous, results)
    }
}
