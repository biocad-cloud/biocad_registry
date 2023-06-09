const wrap_list = function(a) {
    if (is.null(a)) {
        list(count = 0);
    } else {
        list(count = length(a), data = a);
    }
}