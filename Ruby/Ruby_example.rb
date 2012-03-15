require 'rubygems'
require 'curb'

# Usage: <script> username password url
# prints data to stdout.

username, password, url = ARGV.first 3

Curl::Easy.http_get url do |c|
  c.http_auth_types = :basic
  c.username = username
  c.password = password

  c.encoding = "gzip"
  c.verbose = true

  c.on_body do |data|
    puts data
    data.size # required by curl's api.
  end
end