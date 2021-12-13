use std::collections::{HashMap, HashSet};

use crate::parsing::{FromLines, InputParser};

struct Edge {
    first: Vertex,
    second: Vertex,
}

#[derive(Clone, Hash, PartialEq, Eq)]
struct Vertex {
    label: String,
    is_big: bool,
}

type Graph = HashMap<Vertex, Vec<Vertex>>;

pub struct Solution {
    graph: Graph,
}

impl FromLines for Solution {
    fn new(lines: Vec<String>) -> Self {
        let edges: Vec<Edge> = lines
            .into_iter()
            .filter(|string| !string.is_empty())
            .map(|content| {
                let split: Vec<&str> = content.split('-').collect();
                let label = split[0].to_string();
                let is_big = label.chars().all(|char| char.is_uppercase());
                let first = Vertex { label, is_big };
                let label = split[1].to_string();
                let is_big = label.chars().all(|char| char.is_uppercase());
                let second = Vertex { label, is_big };
                Edge { first, second }
            })
            .collect();
        let mut graph = Graph::new();
        for edge in edges {
            if !graph.contains_key(&edge.first) {
                graph.insert(edge.first.clone(), Vec::new());
            }
            if !graph.contains_key(&edge.second) {
                graph.insert(edge.second.clone(), Vec::new());
            }
            let first = graph.get_mut(&edge.first).unwrap();
            first.push(edge.second.clone());
            let second = graph.get_mut(&edge.second).unwrap();
            second.push(edge.first.clone());
        }
        Solution { graph }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 12;

    fn create() -> Self {
        InputParser::from_lines()
    }

    fn solve_first_part(&self) -> String {
        let start = self.graph.keys().find(|&key| key.label == "start").unwrap();
        let mut visited = HashSet::new();
        visited.insert("start");
        let num_of_ways = self.traverse(start, visited, false, HashSet::new());
        num_of_ways.to_string()
    }

    fn solve_second_part(&self) -> String {
        let start = self.graph.keys().find(|&key| key.label == "start").unwrap();
        let mut visited = HashSet::new();
        visited.insert("start");
        let num_of_ways = self.traverse(start, visited, true, HashSet::new());
        num_of_ways.to_string()
    }
}

impl Solution {
    fn traverse(
        &self,
        current: &Vertex,
        visited: HashSet<&str>,
        can_visit_twice: bool,
        visited_once: HashSet<&str>,
    ) -> i32 {
        let neighbours = self.graph.get(current).unwrap();
        let neighbours: Vec<&Vertex> = neighbours
            .iter()
            .filter(|&vertex| {
                let label = vertex.label.as_str();
                vertex.is_big || !visited.contains(&label)
            })
            .collect();
        let mut result = 0;
        for neighbour in neighbours {
            let label = neighbour.label.as_str();
            if label == "end" {
                result += 1;
                continue;
            }
            let mut visited = visited.clone();
            let mut visited_once = visited_once.clone();
            let mut can_visit_twice = can_visit_twice;
            if !neighbour.is_big {
                if can_visit_twice {
                    if visited_once.contains(label) {
                        can_visit_twice = false;
                        for item in visited_once.iter() {
                            visited.insert(item);
                        }
                        visited_once.clear();
                    } else {
                        visited_once.insert(label);
                    }
                } else {
                    visited.insert(label);
                }
            }
            let paths = self.traverse(neighbour, visited, can_visit_twice, visited_once);
            result += paths;
        }
        result
    }
}
