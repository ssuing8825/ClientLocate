SSC.define("SSC.controls.briefcase");

(function() {
    var enteredTags = function(value) {
        return value.split(/\s*,\s*/);
    };
    SSC.controls.briefcase.addTagAutocomplete = function(element) {
        element.keydown(function(event) {
            if (event.keyCode === $.ui.keyCode.TAB && $(this).data("autocomplete").menu.active) {
                event.preventDefault();
            }
        })
        .autocomplete({
            search: function() {
                var lastValue = enteredTags(this.value).pop();
                if (lastValue.length === 0) {
                    element.autocomplete("close");
                }
                if (lastValue.length < 2) {
                    return false;
                }
            },
            source: function(request, respond) {
                $.ajax({
                    type: "GET",
                    url: "/controls/briefcase/suggest-tags",
                    data: { "tag-prefix": enteredTags(request.term).pop() },
                    success: function(json) {
                        respond(json);
                    },
                    error: function() {
                        respond([]);
                    },
                    dataType: "json"
                });
            },
            focus: function() {
                return false;
            },
            select: function(event, ui) {
                this.focus();
                var tags = enteredTags(this.value);
                tags.pop();
                tags.push(ui.item.value);
                tags.push("");
                this.value = tags.join(", ");
                this.scrollTop = this.scrollHeight;

                return false;
            }
        });
    };
})();

$(document).ready(function() {
    $(".add-to-briefcase").each(function() {
        var element = $(this),
            contentItemId = element.attr("data-content-item-id"),
            forumTopicId = element.attr("data-forum-topic-id"),
            communityServerBlogPostId = element.attr("data-cs-blog-post-id"),
            popup,
            getTagsInput = function() {
                return popup.find('form textarea[name="tags"]');
            },
            loadingImage = '<img src="/Resources/Images/loading.gif" alt="" />',
            openPopup = function() {
                popup.show();
            },
            closePopup = function() {
                popup.hide();
            },
            onError = function() {
                updatePopup("<p>An error has occurred.</p>");
            },
            updatePopup = function(html) {
                popup.html('<div class="close-button"><img src="/Resources/Images/close.png" alt="Close" /></div>');
                popup.append(html);
                popup.find(".briefcase-save").click(onSave);
                popup.children(".close-button").click(closePopup);
                SSC.controls.briefcase.addTagAutocomplete(getTagsInput());
            },
            onSave = function() {
                var status = popup.find(".add-to-briefcase-status");
                status.html(loadingImage + " Saving...");
                post({
                    update: true,
                    notes: popup.find('form textarea[name="notes"]').val(),
                    tags: getTagsInput().val()
                });
            },
            post = function(data) {
                $.ajax({
                    type: "POST",
                    url: "/controls/briefcase/add-or-edit",
                    data: $.extend({
                        "content-item-id": contentItemId,
                        "forum-topic-id": forumTopicId,
                        "cs-blog-post-id": communityServerBlogPostId,
                        "page-url": location.pathname
                    }, data || {}),
                    dataType: "html",
                    success: function(html) {
                        if (html === "SAVED") {
                            popup.hide();
                        } else {
                            updatePopup(html);
                        }
                    },
                    error: function() {
                        updatePopup("<p>An error has occurred.</p>");
                    }
                });
            };

        element.click(function(event) {
            if (popup === undefined) {
                popup = $(document.createElement("div")).addClass("add-to-briefcase-div");
                $("body").append(popup);
                $(document).keydown(function(event) {
                    if (event.keyCode === 27 && popup.is(":visible")) {
                        closePopup();
                        event.preventDefault();
                    }
                });
            } else if (popup.is(":visible")) {
                closePopup();
                return;
            } else {
                openPopup();
            }
            popup.css({
                position: "absolute",
                left: element.offset().left + (element.outerWidth() / 2),
                top: element.offset().top + element.outerHeight() + 2
            });
            updatePopup(loadingImage + " Loading...");

            post();
            return false;
        });
    });
});