
#' Serialize any object into messagepack
#' 
#' @param x any ``R#`` object
#' @param schema a schema for serialization object x to messagepack.
#' 
#' @return data stream object
#' 
const to_msgpack as function(x, schema = NULL) {
    MessagePack::pack(x, schema);
}

#' Deserialize object from messagepack data 
#' 
#' @param buffer the binary stream data
#' 
#' @return R# object that read from the binary stream 
#'     data in messagepack data format.
#' 
const unpack as function(buffer) {
    MessagePack::unpack(buffer);
}