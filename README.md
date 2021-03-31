DenizenMetaWebsite
------------------

Meta-documentation website for Denizen.

### How To Install/Run

Designed for and tested on Debian Linux.

- Make sure you have `screen` and `dotnet-5-sdk` available
- Add a user for the service (via `adduser` generally, then `su` to that user)
- Clone the git repo (`git clone https://github.com/DenizenScript/DenizenMetaWebsite`) and enter the folder
- Make a folder labeled `config`, inside it make a text file labeled `config.fds`, and add any configuration keys from the sample config below. All keys are optional, a default config would be an empty file.
- Call `./update.sh`
- Will by default open on port 8098. To change this, edit `start.sh`
- It is strongly recommended you run this webserver behind a reverse proxy like Apache2.

### Configuration

```yml
# Set to a webhook token that will cause the website to reload meta documentation from source.
# This can be used by sending a POST request to "<base url>/Webhook/Reload?token=some_token_here"
# Exclude this config key to not have any reloading.
reload-webhook-token: some_token_here
# Set to a list of alternate meta doc source URLs. Exclude this key to use the default source list.
alt-sources:
- https://example.com
```

### Licensing pre-note:

This is an open source project, provided entirely freely, for everyone to use and contribute to.

If you make any changes that could benefit the community as a whole, please contribute upstream.

### The short of the license is:

You can do basically whatever you want, except you may not hold any developer liable for what you do with the software.

### The long version of the license follows:

The MIT License (MIT)

Copyright (c) 2021 The Denizen Script Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
