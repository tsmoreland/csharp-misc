package com.moreland.sample.commandlineecho.cli;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Application implements CommandLineRunner  {

	private Printer printer;

	@Autowired
	public Application(Printer printer) {
		this.printer = printer;
	}

	public static void main(String[] args) {
		SpringApplication.run(Application.class, args);
	}

    @SuppressWarnings({"java:S106"})
    @Override
    public void run(String... args) throws Exception {

		for (var arg : args) {
			printer.writeline(arg);
		}

	}

}
