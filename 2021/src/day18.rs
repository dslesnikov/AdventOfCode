use std::{cell::RefCell, rc::Rc, str::FromStr, string::ParseError};

use crate::parsing::{FromLines, InputParser};

type TreeNode = Rc<RefCell<Node>>;

struct NumberValue {
    value: TreeNode,
}

enum SnailfishNumber {
    Regular(u32),
    Pair(TreeNode, TreeNode),
}

struct Node {
    parent: Option<TreeNode>,
    content: SnailfishNumber,
}

impl NumberValue {
    fn add(&self, rhs: &NumberValue) -> NumberValue {
        let left = deep_copy(self.value.clone());
        let right = deep_copy(rhs.value.clone());
        let parent_node = Node {
            parent: None,
            content: SnailfishNumber::Pair(left.clone(), right.clone()),
        };
        let parent = Rc::new(RefCell::new(parent_node));
        left.borrow_mut().parent = Some(parent.clone());
        right.borrow_mut().parent = Some(parent.clone());
        let mut result = NumberValue { value: parent };
        result.reduce();
        return result;
    }

    fn magnitude(&self) -> u64 {
        self.value.borrow().magnitude()
    }

    fn reduce(&mut self) {
        let mut explode_candidate = find_explode_candidate(self.value.clone());
        let mut was_exploded = false;
        let mut was_split = false;
        while let Some(candidate) = &explode_candidate {
            was_exploded = true;
            let closest_left_value = find_closest_left_value(&candidate);
            let closest_right_value = find_closest_right_value(&candidate);
            explode(candidate.clone(), closest_left_value, closest_right_value);
            explode_candidate = find_explode_candidate(self.value.clone());
        }
        let split_candidate = find_split_candidate(self.value.clone());
        if let Some(split_can) = &split_candidate {
            was_split = true;
            split(split_can.clone());
        }
        if was_exploded || was_split {
            self.reduce();
        }
    }
}

impl FromStr for NumberValue {
    type Err = ParseError;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let node = Node::parse(s);
        Ok(NumberValue { value: node })
    }
}

fn deep_copy(node: TreeNode) -> TreeNode {
    let value = node;
    let content = &value.borrow().content;
    match &content {
        SnailfishNumber::Regular(regular_value) => {
            let val = *regular_value;
            let new_node = Node {
                parent: None,
                content: SnailfishNumber::Regular(val),
            };
            return Rc::new(RefCell::new(new_node));
        }
        SnailfishNumber::Pair(left, right) => {
            let left_copy = deep_copy(left.clone());
            let right_copy = deep_copy(right.clone());
            let parent = Node {
                parent: None,
                content: SnailfishNumber::Pair(left_copy.clone(), right_copy.clone()),
            };
            let parent_rc = Rc::new(RefCell::new(parent));
            left_copy.borrow_mut().parent = Some(parent_rc.clone());
            right_copy.borrow_mut().parent = Some(parent_rc.clone());
            return parent_rc;
        }
    }
}

fn explode(node: TreeNode, left_target: Option<TreeNode>, right_target: Option<TreeNode>) {
    if let SnailfishNumber::Pair(left, right) = &node.borrow().content {
        if let SnailfishNumber::Regular(left_value) = &left.borrow().content {
            if let Some(left_target_node) = left_target {
                if let SnailfishNumber::Regular(left_target_value) =
                    &mut left_target_node.borrow_mut().content
                {
                    *left_target_value += left_value;
                }
            }
        }
        if let SnailfishNumber::Regular(right_value) = &right.borrow().content {
            if let Some(right_target_node) = right_target {
                if let SnailfishNumber::Regular(right_target_value) =
                    &mut right_target_node.borrow_mut().content
                {
                    *right_target_value += right_value;
                }
            }
        }
    }
    node.borrow_mut().content = SnailfishNumber::Regular(0);
}

fn split(node: TreeNode) {
    let mut node_value: Option<u32> = None;
    if let SnailfishNumber::Regular(value) = node.clone().borrow().content {
        node_value = Some(value);
    }
    if let Some(value) = node_value {
        let left_node = Node {
            parent: Some(node.clone()),
            content: SnailfishNumber::Regular(value / 2),
        };
        let left = Rc::new(RefCell::new(left_node));
        let right_node = Node {
            parent: Some(node.clone()),
            content: SnailfishNumber::Regular((value + 1) / 2),
        };
        let right = Rc::new(RefCell::new(right_node));
        node.borrow_mut().content = SnailfishNumber::Pair(left, right);
    }
}

fn find_explode_candidate(node: TreeNode) -> Option<TreeNode> {
    find_explode_candidate_internal(node, 0)
}

fn find_explode_candidate_internal(node: TreeNode, depth: usize) -> Option<TreeNode> {
    if depth >= 4 {
        if let SnailfishNumber::Pair(left, right) = &node.borrow().content {
            if let SnailfishNumber::Regular(_) = &left.borrow().content {
                if let SnailfishNumber::Regular(_) = &right.borrow().content {
                    return Some(node.clone());
                }
            }
        }
    }
    match &node.borrow_mut().content {
        SnailfishNumber::Regular(_) => {
            return None;
        }
        SnailfishNumber::Pair(left, right) => {
            let left_candidate = find_explode_candidate_internal(left.clone(), depth + 1);
            if left_candidate.is_some() {
                return left_candidate;
            }
            let right_candidate = find_explode_candidate_internal(right.clone(), depth + 1);
            if right_candidate.is_some() {
                return right_candidate;
            }
            return None;
        }
    }
}

fn find_split_candidate(node: TreeNode) -> Option<TreeNode> {
    match &node.borrow_mut().content {
        SnailfishNumber::Regular(value) => {
            if *value >= 10 {
                return Some(node.clone());
            }
            return None;
        }
        SnailfishNumber::Pair(left, right) => {
            let left_candidate = find_split_candidate(left.clone());
            if left_candidate.is_some() {
                return left_candidate;
            }
            let right_candidate = find_split_candidate(right.clone());
            if right_candidate.is_some() {
                return right_candidate;
            }
            return None;
        }
    }
}

fn find_closest_left_value(node: &TreeNode) -> Option<TreeNode> {
    let mut current = node.clone();
    let mut parent = node.borrow().parent.clone();
    while let Some(parent_node) = parent {
        let mut child: TreeNode;
        match &parent_node.borrow().content {
            SnailfishNumber::Regular(_) => panic!(),
            SnailfishNumber::Pair(left, _) => {
                if current.as_ptr() == left.as_ptr() {
                    current = parent_node.clone();
                    parent = parent_node.clone().borrow().parent.clone();
                    continue;
                }
                child = left.clone();
            }
        }
        while let SnailfishNumber::Pair(_, right) = &child.clone().borrow().content {
            child = right.clone();
        }
        return Some(child);
    }
    return None;
}

fn find_closest_right_value(node: &TreeNode) -> Option<TreeNode> {
    let mut current = node.clone();
    let mut parent = node.borrow().parent.clone();
    while let Some(parent_node) = parent {
        let mut child: TreeNode;
        match &parent_node.borrow().content {
            SnailfishNumber::Regular(_) => panic!(),
            SnailfishNumber::Pair(_, right) => {
                if current.as_ptr() == right.as_ptr() {
                    current = parent_node.clone();
                    parent = parent_node.clone().borrow().parent.clone();
                    continue;
                }
                child = right.clone();
            }
        }
        while let SnailfishNumber::Pair(left, _) = &child.clone().borrow().content {
            child = left.clone();
        }
        return Some(child);
    }
    return None;
}

impl Node {
    fn magnitude(&self) -> u64 {
        match &self.content {
            SnailfishNumber::Regular(value) => *value as u64,
            SnailfishNumber::Pair(left, right) => {
                let left_magnitude = left.borrow().magnitude();
                let right_magnitude = right.borrow().magnitude();
                return 3 * left_magnitude + 2 * right_magnitude;
            }
        }
    }

    fn parse(content: &str) -> TreeNode {
        let chars: Vec<char> = content.chars().collect();
        let result = Self::parse_next(&chars).0;
        result
    }

    fn parse_next(content: &[char]) -> (TreeNode, usize) {
        let current = content[0];
        if let Some(digit) = current.to_digit(10) {
            let node = Node {
                parent: None,
                content: SnailfishNumber::Regular(digit),
            };
            let result = Rc::new(RefCell::new(node));
            return (result, 1);
        }
        let mut parsed = 1;
        let parsed_left = Self::parse_next(&content[parsed..]);
        let left = parsed_left.0;
        parsed += parsed_left.1 + 1;
        let parsed_right = Self::parse_next(&content[parsed..]);
        let right = parsed_right.0;
        parsed += parsed_right.1;
        let pair = SnailfishNumber::Pair(left.clone(), right.clone());
        let node = Node {
            parent: None,
            content: pair,
        };
        let parent = Rc::new(RefCell::new(node));
        left.borrow_mut().parent = Some(parent.clone());
        right.borrow_mut().parent = Some(parent.clone());
        return (parent, parsed + 1);
    }
}

pub struct Solution {
    numbers: Vec<NumberValue>,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let numbers: Vec<NumberValue> = lines
            .into_iter()
            .filter(|line| !line.is_empty())
            .map(|content| content.parse().unwrap())
            .collect();
        Solution { numbers }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 18;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let mut result: NumberValue;
        let mut sum_result = &self.numbers[0];
        for number in self.numbers.iter().skip(1) {
            result = sum_result.add(number);
            sum_result = &result;
        }
        let magnitude = sum_result.magnitude();
        magnitude.to_string()
    }

    fn solve_second_part(&self) -> String {
        let mut max_sum = 0;
        let length = self.numbers.len();
        for first in 0..length {
            for second in (first + 1)..length {
                let first = &self.numbers[first];
                let second = &self.numbers[second];
                let right_sum = first.add(second).magnitude();
                let left_sum = second.add(first).magnitude();
                max_sum = right_sum.max(max_sum);
                max_sum = left_sum.max(max_sum);
            }
        }
        max_sum.to_string()
    }
}
