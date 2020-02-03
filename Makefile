setup:
	cd Xcode && make proj
	./Rust/replace_template_bin setting.yml Unity/Makefile_template Unity/Makefile