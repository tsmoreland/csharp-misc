#include "pch.h"
#include "app_setting_consumer.h"
#include "../ManagedLibrary/AppSettingsProvider.h"

#include <iostream>


namespace native_library
{

app_setting_consumer::app_setting_consumer()
	: m_dumpFiles(false)
{
	auto const value = ManagedLibrary::get_appsetting_value("EnableCrashDumps");
	m_dumpFiles = (value.value_or("") == "true");
}

bool app_setting_consumer::get_dump_files_enabled() const
{
	return m_dumpFiles;
}

}

