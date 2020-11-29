package com.moreland.sample.commandlineecho.cli;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class ApplicationConfiguration {
    
    @Bean
    public Printer printer() {
        return new ConsolePrinter();
    }
}
