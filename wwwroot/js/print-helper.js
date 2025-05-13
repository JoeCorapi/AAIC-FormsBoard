window.printHelper = {
    printElement: function (element) {
        // Save the current body content
        var originalContent = document.body.innerHTML;

        // Get the content of the element to print
        var printContent = element.innerHTML;

        // Replace the body content with just what we want to print
        document.body.innerHTML = printContent;

        // Print the page
        window.print();

        // Restore the original content
        document.body.innerHTML = originalContent;

        // Reload any JavaScript that might have been affected
        window.location.reload();
    }
};