#include "pch.h"
#include "AppSettingsProvider.h"
#include <msclr/marshal_cppstd.h>

using std::string;
using std::optional;
using std::nullopt;

using namespace System;
using namespace System::Configuration;
using namespace System::Collections::Specialized;

namespace ManagedLibrary
{

optional<string> get_appsetting_value(string const& key)
{
	if (key.empty()) {
		return nullopt;
	}

	String^ managedKey = gcnew String(key.c_str());
	NameValueCollection^ appSettings = ConfigurationManager::AppSettings;
	if (appSettings == __nullptr) {
		return nullopt;
	}

	String^ value = appSettings[managedKey];
	if (String::IsNullOrWhiteSpace(value)) {
		return nullopt;
	}
	
	msclr::interop::marshal_context context;

	auto native_value = context.marshal_as<string>(value);

	return native_value;
}
	
}
