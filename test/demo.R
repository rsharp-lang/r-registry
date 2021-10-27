# # Rmsgpack
# Messagepack data read/write code module for R# programming language.
# 
# [MessagePack](http://msgpack.org/) is an efficient binary serialization format. It lets you 
# exchange data among multiple languages like JSON but it's faster and 
# smaller. For example, small integers (like flags or error code) are 
# encoded into a single byte, and typical short strings only require an 
# extra byte in addition to the strings themselves.
;

#region "demo code"
require(msgpack);

msg = [1,2,3] |> to_msgpack;
vec = msgpack::unpack(msg);
#end region
;
