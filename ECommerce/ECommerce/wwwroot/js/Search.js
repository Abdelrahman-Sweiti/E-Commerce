
<script>
    $(document).ready(function () {
        $("#search-input").on("input", function () {
            // Get the user's input
            var query = $(this).val();

            // Check if the query is not empty
            if (query.length > 0) {
                // Send an AJAX request to fetch suggestions
                $.ajax({
                    url: "@Url.Action("GetSearchSuggestions", "Products")",
                    data: { query: query },
                    method: "GET",
                    success: function (data) {
                        // Clear the previous suggestions
                        $("#search-suggestions").empty();

                        // Add new suggestions to the dropdown
                        $.each(data, function (index, suggestion) {
                            $("#search-suggestions").append(
                                '<a class="dropdown-item" href="#">' + suggestion + '</a>'
                            );
                        });

                        // Show the dropdown
                        $("#search-suggestions").addClass("show");
                    }
                });
            } else {
                // If the input is empty, hide the suggestions dropdown
                $("#search-suggestions").removeClass("show");
            }
        });
    });
</script>
