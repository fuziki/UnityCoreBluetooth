extern crate yaml_rust;
use std::env;
use std::fs::File;
use std::io::{Read, BufRead, BufReader, Write, BufWriter};
use yaml_rust::{yaml, YamlLoader};

fn main() {
    println!("Hello, world!");

    let args: Vec<String> = env::args().collect();
    println!("args {:?}", args);

    let setting_file_name: &str = &args[1];
    let input_file_name: &str = &args[2];
    let output_file_name: &str = &args[3];

    let setting_file: std::fs::File = File::open(setting_file_name).expect("filed to read input file");
    let mut setting_reader: std::io::BufReader<std::fs::File> = BufReader::new(setting_file);
    let mut setting_string: std::string::String = String::new();
    setting_reader.read_to_string(&mut  setting_string).expect("filed read string");

    let setting_loader: std::vec::Vec<yaml_rust::Yaml> = YamlLoader::load_from_str(&setting_string).expect("filed to load from str");
    let setting_yml: &yaml_rust::Yaml = &setting_loader[0];

    let mut setting_list: std::collections::HashMap<std::string::String, std::string::String>;

    match *setting_yml {
        yaml::Yaml::Hash(ref h) => {
            setting_list = h.iter().map(|(k, v)| -> (std::string::String, std::string::String) {
                    let k: &str = k.as_str().expect("filed to get project name");
                    let v: &str = v.as_str().expect("filed to get project name");
                    (k.to_owned(), v.to_owned())
                }
            ).collect();
        }
        _ => {
            return;
        }
    }
    println!("{:?}", setting_list);

    setting_list.insert("DESCRIPTION".to_string(), "Don't edit this file!!".to_string());

    let input_file: std::fs::File = File::open(input_file_name).expect("filed to read input file");
    let input_reader: std::io::BufReader<std::fs::File> = BufReader::new(input_file);
    let input_lines: std::io::Lines<std::io::BufReader<std::fs::File>> = input_reader.lines();

    let output_file: std::fs::File = File::create(output_file_name).expect("failed to create output file");
    let mut output_writer: std::io::BufWriter<std::fs::File> = BufWriter::new(output_file);    

    for line in input_lines {
        let mut line: std::string::String = line.expect("failed to read line");
        for (k, v) in &setting_list {
            line = line.replace(&format!("{{{{{}}}}}", k), &v);
        }
        let line: std::string::String = line + "\n";
        print!("{}", line);
        output_writer.write(line.as_bytes()).expect("failed to read line");
    }
}
