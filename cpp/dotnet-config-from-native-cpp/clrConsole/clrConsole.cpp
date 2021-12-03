#include "pch.h"
#include "..//NativeLibrary/app_setting_consumer.h"

using namespace System;
using namespace System::Configuration;
using namespace System::Collections::Specialized;

int main(array<System::String ^> ^args)
{
	NameValueCollection^ appSettings = ConfigurationManager::AppSettings;

    // should null check first
	String^ value = appSettings["EnableCrashDumps"];
    
    Console::WriteLine(value);

    native_library::app_setting_consumer const consumer;
	
    Console::WriteLine(consumer.get_dump_files_enabled());

    return 0;
}
