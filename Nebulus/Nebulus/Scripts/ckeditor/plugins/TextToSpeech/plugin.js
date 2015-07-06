(function() {
    var o = {
        exec: function (p) {
            var selectedRanges = p.getSelection().getRanges()[0];
            if (!selectedRanges.collapsed) {
                var style = new CKEDITOR.style({ element: 'span', attributes: { 'class': 'Speech' } });

                if (style.checkActive(p.elementPath(), p))
                {
                    p.removeStyle(style);
                }
                else
                    p.applyStyle(style);
            }
    }};
    CKEDITOR.plugins.add('TextToSpeech', {
        icons: 'TextToSpeech',
        init: function (editor) {
            editor.addCommand('TextToSpeech', o);
            editor.ui.addButton('TextToSpeech', {
                label: 'TextToSpeech',
                command: 'TextToSpeech'
            });
        }
    });
})();
