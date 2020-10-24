#!/bin/sh

$JDK_HOME/bin/java --module-path math/target/math-1.0-SNAPSHOT.jar;math-simple/target/math-simple-1.0-SNAPSHOT.jar;math-cli/target/math-cli-1.0-SNAPSHOT.jar; -m moreland.sample.modules.math.cli/moreland.sample.modules.math.cli.Application
