package com.moreland.sample.commandlineecho.cli;

public class ConsolePrinter implements Printer {

    @SuppressWarnings({"java:S106"})
    @Override
    public void writeline(String message) {
        System.out.println(message);
    }
    
}
