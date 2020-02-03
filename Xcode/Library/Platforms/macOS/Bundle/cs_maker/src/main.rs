use std::env;
use std::fs::File;
use std::io::{BufRead, BufReader, Write, BufWriter};

fn main() {
    let args: Vec<String> = env::args().collect();
    println!("args {:?}", args);

    let project_name: &str = &args[1];
    let target_name: &str = &args[2];
    let product_name: &str = &args[3];

    let input_file_name: &str = "../NativePluginBundle.template.cs";
    let input_file: std::fs::File = File::open(input_file_name).expect("filed to read input file");
    let input_reader: std::io::BufReader<std::fs::File> = BufReader::new(input_file);
    let input_lines: std::io::Lines<std::io::BufReader<std::fs::File>> = input_reader.lines();

    let output_file_name: std::string::String = format!("../{}.cs", target_name);
    let output_file: std::fs::File = File::create(&output_file_name).expect("failed to create output file");
    let mut output_writer: std::io::BufWriter<std::fs::File> = BufWriter::new(output_file);    

    for line/*: std::result::Result<std::string::String, std::io::Error> */ in input_lines {
        let line: std::string::String = line.expect("failed to read line");
        let line: std::string::String = line.replace("{{PROJECT_NAME}}", project_name);
        let line: std::string::String = line.replace("{{TARGET_NAME}}", target_name);
        let line: std::string::String = line.replace("{{PRODUCT_NAME}}", product_name);
        let line: std::string::String = line + "\n";
        print!("{}", line);
        output_writer.write(line.as_bytes()).expect("failed to read line");
    }
    
}
