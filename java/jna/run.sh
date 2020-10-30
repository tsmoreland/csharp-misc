#!/bin/sh

$JDK_HOME/bin/java --module-path interop-app/target/interop-app-1.0-SNAPSHOT.jar;interop-win32-ansi/target/interop-win32-ansi-1.0-SNAPSHOT.jar;interop-service/target/interop-service-1.0-SNAPSHOT.jar; -m moreland.sample.jna.interop.app/moreland.sample.jna.interop.app.Application