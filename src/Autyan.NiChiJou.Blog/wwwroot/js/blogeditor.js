$(function() {
    $('form#articleEditor').submit(function () {
        $('#Extract').val(tinyMCE.activeEditor.getContent({ format: 'text' }).substring(0, 200));
        return true;
    });

    $(document).ready(function () {
        tinymce.init({
            selector: "#biyoriEditor",
            plugins: "code, image, imagetools, lists, advlist, media, link, table, textcolor, emoticons",
            menubar: "file, edit, view, format, insert, table",
            toolbar: "insertfile a11ycheck undo redo | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link image | help, code, image, media, link, forecolor backcolor, emoticons",
            advlist_bullet_styles: "default,lower-alpha,lower-greek,lower-roman,upper-alpha,upper-roman",
            textcolor_map: [
                "000000", "Black",
                "993300", "Burnt orange",
                "333300", "Dark olive",
                "003300", "Dark green",
                "003366", "Dark azure",
                "000080", "Navy Blue",
                "333399", "Indigo",
                "333333", "Very dark gray",
                "800000", "Maroon",
                "FF6600", "Orange",
                "808000", "Olive",
                "008000", "Green",
                "008080", "Teal",
                "0000FF", "Blue",
                "666699", "Grayish blue",
                "808080", "Gray",
                "FF0000", "Red",
                "FF9900", "Amber",
                "99CC00", "Yellow green",
                "339966", "Sea green",
                "33CCCC", "Turquoise",
                "3366FF", "Royal blue",
                "800080", "Purple",
                "999999", "Medium gray",
                "FF00FF", "Magenta",
                "FFCC00", "Gold",
                "FFFF00", "Yellow",
                "00FF00", "Lime",
                "00FFFF", "Aqua",
                "00CCFF", "Sky blue",
                "993366", "Red violet",
                "FFFFFF", "White",
                "FF99CC", "Pink",
                "FFCC99", "Peach",
                "FFFF99", "Light yellow",
                "CCFFCC", "Pale green",
                "CCFFFF", "Pale cyan",
                "99CCFF", "Light sky blue",
                "CC99FF", "Plum"
            ]
        });
    });
});