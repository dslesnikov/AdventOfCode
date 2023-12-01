use std::{cmp::Ordering, str::FromStr};

use crate::FromContent;

#[derive(Clone)]
enum Packet {
    Constant(u8),
    List(Vec<Packet>),
}

impl PartialEq for Packet {
    fn eq(&self, other: &Self) -> bool {
        match (self, other) {
            (Self::List(left), Self::List(right)) => {
                if left.len() != right.len() {
                    return false;
                }
                left.iter().zip(right.iter()).all(|(l, r)| l.eq(r))
            }
            (Self::Constant(left), Self::Constant(right)) => left == right,
            _ => false,
        }
    }
}

impl Eq for Packet {}

impl PartialOrd for Packet {
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        match (self, other) {
            (Packet::Constant(left), Packet::Constant(right)) => left.partial_cmp(right),
            (Packet::Constant(left_value), Packet::List(_)) => {
                let left = Packet::List(vec![Packet::Constant(*left_value)]);
                left.partial_cmp(other)
            }
            (Packet::List(_), Packet::Constant(right_value)) => {
                let right = Packet::List(vec![Packet::Constant(*right_value)]);
                self.partial_cmp(&right)
            }
            (Packet::List(left_list), Packet::List(right_list)) => {
                if left_list.is_empty() && right_list.is_empty() {
                    return Some(Ordering::Equal);
                }
                if left_list.is_empty() {
                    return Some(std::cmp::Ordering::Less);
                }
                if right_list.is_empty() {
                    return Some(std::cmp::Ordering::Greater);
                }
                let ordering = left_list[0].partial_cmp(&right_list[0]);
                if let Some(Ordering::Greater) = ordering {
                    return ordering;
                }
                if let Some(Ordering::Less) = ordering {
                    return ordering;
                }
                let mut new_left: Vec<Packet> = left_list.clone();
                new_left.remove(0);
                let mut new_right = right_list.clone();
                new_right.remove(0);
                return Packet::List(new_left).partial_cmp(&Packet::List(new_right));
            }
        }
    }
}

impl std::cmp::Ord for Packet {
    fn cmp(&self, other: &Self) -> std::cmp::Ordering {
        self.partial_cmp(other).unwrap()
    }
}

impl FromStr for Packet {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let (packet, _) = parse_next_packet(s);
        Ok(packet)
    }
}

fn parse_next_packet(s: &str) -> (Packet, usize) {
    let bytes = s.as_bytes();
    if bytes[0].is_ascii_digit() {
        let digit_length = if bytes.len() > 1 && bytes[1].is_ascii_digit() {
            2
        } else {
            1
        };
        return (
            Packet::Constant(s[0..digit_length].parse().unwrap()),
            digit_length,
        );
    }
    if bytes[0] == '[' as u8 {
        let mut parsed = 1;
        let mut packets: Vec<Packet> = Vec::new();
        let mut slice = &s[1..];
        let mut byte_slice = slice.as_bytes();
        while byte_slice[0] != ']' as u8 {
            let (packet, mut parsed_length) = parse_next_packet(slice);
            packets.push(packet);
            if byte_slice[parsed_length] == ',' as u8 {
                parsed_length += 1;
            }
            parsed += parsed_length;
            slice = &slice[parsed_length..];
            byte_slice = slice.as_bytes();
        }
        parsed += 1;
        return (Packet::List(packets), parsed);
    }
    panic!()
}

struct PacketPair {
    first: Packet,
    second: Packet,
}

pub(crate) struct Solution {
    pairs: Vec<PacketPair>,
}

impl FromContent for Solution {
    fn from_content(content: String) -> Self {
        let pairs = content
            .split("\n\n")
            .map(|pair| {
                let split: Vec<&str> = pair.split('\n').collect();
                let first = split[0].parse().unwrap();
                let second = split[1].parse().unwrap();
                PacketPair { first, second }
            })
            .collect();
        Self { pairs }
    }
}

impl crate::Solution for Solution {
    const DAY: u32 = 13;

    fn solve_first_part(&self) -> String {
        let mut result = 0;
        for i in 0..self.pairs.len() {
            let pair = &self.pairs[i];
            if pair.first < pair.second {
                result += i + 1;
            }
        }
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let packets: Vec<&Packet> = self
            .pairs
            .iter()
            .flat_map(|pair| vec![&pair.first, &pair.second])
            .collect();
        let two: Packet = "[[2]]".parse().unwrap();
        let less_than_two = packets.iter().filter(|p| p < &&&two).count();
        let six: Packet = "[[6]]".parse().unwrap();
        let more_than_six = packets.iter().filter(|p| p > &&&six).count();
        let two_index = less_than_two + 1;
        let six_index = packets.len() - more_than_six + 2;
        (two_index * six_index).to_string()
    }
}
