#pragma once

#include "exports.h"

namespace native_library
{
	class NATIVELIBRARY_EXPORT app_setting_consumer final
	{
		bool m_dumpFiles;
	public:
		explicit app_setting_consumer();

		[[nodiscard]]
		bool get_dump_files_enabled() const;
	};
	
}

