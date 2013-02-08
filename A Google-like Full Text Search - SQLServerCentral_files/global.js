function ssc_showPostsSinceLastVisit() {
	// Make a form we can post
	// (don't use the existing one as it contains unhelpful VIEWSTATE info and its name varies)
	var form = document.createElement("FORM");
	form.action = "/Forums/Default.aspx";
	form.method = "POST";
	form.style.display = "none";
	document.body.appendChild(form);
	var target = form.appendChild(document.createElement("INPUT"));
	target.name = "__EVENTTARGET";
	target.value = "smRecentPosts";
	var argument = form.appendChild(document.createElement("INPUT"));
	argument.name = "__EVENTARGUMENT";
	argument.value = "butViewPostsSinceLastVisit";
	form.submit();
}

$(document).ready(function() {
    if (location.pathname === "/") {
        setTimeout(function() {
            $("#corner-peel img").stop().animate({
                width: '120px',
                height: '120px'
            }, 500);

            $("#corner-peel-behind").stop().animate({
                width: '120px',
                height: '120px'
            }, 500, function() {
                setTimeout(function() {
                    $("#corner-peel img").stop().animate({
                        width: '82px',
                        height: '82px'
                    }, 220);
                    $("#corner-peel-behind").stop().animate({
                        width: '80px',
                        height: '80px'
                    }, 200);
                }, 1000);
            });
        }, 1000);
    }

    $("#corner-peel").hover(function() {
        $("#corner-peel img, #corner-peel-behind").stop().animate({
            width: '307px',
            height: '319px'
        }, 500);
    }, function() {
        $("#corner-peel img").stop().animate({
            width: '82px',
            height: '82px'
        }, 220);
        $("#corner-peel-behind").stop().animate({
            width: '80px',
            height: '80px'
        }, 200); //Note this one retracts a bit faster (to prevent glitching in IE)
    });

});