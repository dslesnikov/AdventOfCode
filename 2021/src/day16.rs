use crate::parsing::{FromContent, InputParser};

struct Packet {
    version: u16,
    type_id: u16,
    content: PacketContent,
}

enum PacketContent {
    LiteralPacket(u64),
    OperatorPacket(Vec<Packet>),
}

fn to_num(bytes: &[u8]) -> u16 {
    let mut result: u16 = 0;
    for &bit in bytes {
        result = (result << 1) | bit as u16;
    }
    result
}

impl Packet {
    fn parse(content: &[u8]) -> Packet {
        Self::parse_next(content).0
    }

    fn parse_next(content: &[u8]) -> (Packet, usize) {
        let version = to_num(&content[0..3]);
        let type_id = to_num(&content[3..6]);
        if type_id == 4 {
            let mut start = 6;
            let mut group = &content[start..(start + 5)];
            let mut number = to_num(&group[1..]);
            let mut result: u64 = 0;
            while group[0] == 1 {
                result = (result << 4) | number as u64;
                start += 5;
                group = &content[start..(start + 5)];
                number = to_num(&group[1..]);
            }
            result = (result << 4) | number as u64;
            let packet = Packet {
                version,
                type_id,
                content: PacketContent::LiteralPacket(result),
            };
            return (packet, start + 5);
        }
        if content[6] == 0 {
            let mut packets = Vec::new();
            let length_slice = &content[7..22];
            let length = to_num(length_slice) as usize;
            let mut parsed_length: usize = 0;
            let mut slice_start = 22;
            while parsed_length != length {
                let (packet, parsed) = Self::parse_next(&content[slice_start..]);
                parsed_length += parsed;
                packets.push(packet);
                slice_start += parsed;
            }
            let packet = Packet {
                version,
                type_id,
                content: PacketContent::OperatorPacket(packets),
            };
            return (packet, slice_start);
        }
        let mut packets = Vec::new();
        let length = to_num(&content[7..18]);
        let mut packets_parsed = 0;
        let mut slice_start = 18;
        while packets_parsed != length {
            let (packet, parsed) = Self::parse_next(&content[slice_start..]);
            packets_parsed += 1;
            packets.push(packet);
            slice_start += parsed;
        }
        let packet = Packet {
            version,
            type_id,
            content: PacketContent::OperatorPacket(packets),
        };
        return (packet, slice_start);
    }
}

pub struct Solution {
    packet: Packet,
}

impl FromContent for Solution {
    fn new(content: String) -> Self {
        let binary = content
            .trim()
            .chars()
            .fold(Vec::<u8>::new(), |mut acc, char| {
                let digit = char.to_digit(16).unwrap();
                acc.push(((digit >> 3) & 1) as u8);
                acc.push(((digit >> 2) & 1) as u8);
                acc.push(((digit >> 1) & 1) as u8);
                acc.push((digit & 1) as u8);
                acc
            });
        let packet = Packet::parse(&binary);
        Solution { packet }
    }
}

impl crate::Solution for Solution {
    const DAY: i32 = 16;

    fn create() -> Self {
        InputParser::from_content()
    }

    fn solve_first_part(&self) -> String {
        let result = Self::sum_version_ids(&self.packet);
        result.to_string()
    }

    fn solve_second_part(&self) -> String {
        let result = Self::calculate_packet(&self.packet);
        result.to_string()
    }
}

impl Solution {
    fn sum_version_ids(packet: &Packet) -> u32 {
        match &packet.content {
            PacketContent::LiteralPacket(_) => packet.version as u32,
            PacketContent::OperatorPacket(packets) => {
                (packet.version as u32)
                    + packets
                        .iter()
                        .map(|p| Self::sum_version_ids(p))
                        .sum::<u32>()
            }
        }
    }

    fn calculate_packet(packet: &Packet) -> u64 {
        match &packet.content {
            PacketContent::LiteralPacket(value) => *value as u64,
            PacketContent::OperatorPacket(packets) => {
                let calculated: Vec<u64> = packets.iter().map(Self::calculate_packet).collect();
                match packet.type_id {
                    0 => calculated.iter().sum(),
                    1 => calculated.iter().product(),
                    2 => *calculated.iter().min().unwrap(),
                    3 => *calculated.iter().max().unwrap(),
                    5 => (calculated[0] > calculated[1]) as u64,
                    6 => (calculated[0] < calculated[1]) as u64,
                    7 => (calculated[0] == calculated[1]) as u64,
                    _ => panic!("Unknown packet type"),
                }
            }
        }
    }
}
