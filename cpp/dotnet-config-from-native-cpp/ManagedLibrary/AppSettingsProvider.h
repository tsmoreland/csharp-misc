#pragma once

#include <string>
#include <optional>
#include "ManagedExport.h"

namespace ManagedLibrary
{
	[[nodiscard]]
	MANAGEDLIBRARY_EXPORT std::optional<std::string> __cdecl get_appsetting_value(std::string const& key);

}
