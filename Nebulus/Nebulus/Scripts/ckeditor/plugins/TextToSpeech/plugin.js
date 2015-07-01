(function() {
    var o = { exec: function(p) {
        url = baseUrl + "/GetSomeData";
        $.post(url, function(response) {
            alert(response)
        });
    }
    };
    CKEDITOR.plugins.add('TextToSpeech', {
        init: function(editor) {
            editor.addCommand('TextToSpeech', o);
            editor.ui.addButton('TextToSpeech', {
                label: 'TextToSpeech',
                icon: this.path + 'TextToSpeech.png',
                command: 'TextToSpeech'
            });
        }
    });
})();
