//
// Copyright © 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
package moreland.sample.jna.interop.app;

import moreland.sample.jna.interop.service.MessageBox;
import moreland.sample.jna.interop.service.MessageBoxType;

import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Objects;
import java.util.ServiceLoader;

class Application {
    public static void main(String[] args) {

        List<MessageBox> messsageBoxServices = new ArrayList<>();
        ServiceLoader
            .load(MessageBox.class)
            .forEach(messsageBoxServices::add);

        var first = messsageBoxServices.size() > 0 
            ? messsageBoxServices.get(0)
            : null;
        if (Objects.isNull(first)) {
            System.out.println("No messagebox services found.");
            return;
        } else {
            System.out.println(String.format("Found %d services, using: %s", messsageBoxServices.size(), first.getClass().getName()));
        }

        first.display("Body", "Title", EnumSet.of(MessageBoxType.OK));
    }
    
}
