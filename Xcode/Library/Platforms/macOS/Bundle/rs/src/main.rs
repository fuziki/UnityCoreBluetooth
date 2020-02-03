extern crate rand;

use std::fs::File;
use std::io::{BufRead, BufReader, Write, BufWriter};
use rand::Rng;

fn main() {
    let input_file_name: &str = "../macOS.template.xcconfig";
    let input_file: std::fs::File = File::open(input_file_name).expect("filed to read input file");
    let input_reader: std::io::BufReader<std::fs::File> = BufReader::new(input_file);
    let input_lines: std::io::Lines<std::io::BufReader<std::fs::File>> = input_reader.lines();

    let output_file_name: &str = "../macOS.xcconfig";
    let output_file: std::fs::File = File::create(output_file_name).expect("failed to create output file");
    let mut output_writer: std::io::BufWriter<std::fs::File> = BufWriter::new(output_file);    

    for input_line/*: std::result::Result<std::string::String, std::io::Error> */ in input_lines {
        let input_line: std::string::String = input_line.expect("failed to read line");
        let uuid = "_".to_owned() + &get_uuid();
        let out_line: std::string::String = input_line.replace("{{UNIQUEIDXX}}", &uuid);
        let out_line: std::string::String = out_line.replace("{{DESCRIPTIONS}}", "Don't edit this file!!") + "\n";
        println!("{}", out_line);
        output_writer.write(out_line.as_bytes()).expect("failed to read line");
    }
}

fn get_uuid() -> String {
    const CHARSET: &[u8] = b"ABCDEFGHIJKLMNOPQRSTUVWXYZ\
                            abcdefghijklmnopqrstuvwxyz\
                            0123456789";
    const PASSWORD_LEN: usize = 10;
    let mut rng = rand::thread_rng();

    let password: String = (0..PASSWORD_LEN)
        .map(|_| {
            let idx = rng.gen_range(0, CHARSET.len());
            CHARSET[idx] as char
        })
        .collect();

    return password;
}