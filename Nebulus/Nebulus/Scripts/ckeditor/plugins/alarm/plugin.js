(function() {
    var o = {
        exec: function (p) {
           
    }};
    CKEDITOR.plugins.add('alarm', {
        icons: 'alarm',
        init: function (editor) {
            editor.addCommand('alarm', o);
            editor.ui.addButton('alarm', {
                toolbar : 'audio',
                label: 'Alarm',
                command: 'alarm'
            });
        }
    });
})();
