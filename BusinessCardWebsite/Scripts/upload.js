
function clickUpload(id, func) {

    var files = $(id)[0].files;
    for (let i = 0; i < files.length; i++) {
        requestUploadFile(files[i], func);
    }
    $(id).val('');
}


function requestUploadFile(file, func) {
    var url = '/Project/UploadFile';
    formData = new FormData();
    formData.append("uploadedFile", file);
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (fileUrl) {
            func(fileUrl);
        },
        error: function (data) {
        }
    });
}

function requestRemoveFile(thisElement, fileName) {
    var url = '/Project/DeleteFile';
    formData = new FormData();
    formData.append("fileName", fileName);
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            $(thisElement).parent().remove();
        },
        error: function (data) {
        }
    });
}

function requestRemoveFileDb(thisElement, fileName) {
    var url = '/Project/DeleteFileDb';
    formData = new FormData();
    formData.append("fileName", fileName);
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            $(thisElement).parent().remove();
        },
        error: function (data) {
        }
    });
}



$('#content').summernote({
    height: 300,
    minHeight: null,
    maxHeight: null,
    callbacks: {
        onImageUpload: function (files) {
            for (let i = 0; i < files.length; i++) {
                requestUploadFile(files[i], addImgToSummernote);
            }
        }
    }
});

function addImgToSummernote(FileUrl) {
    var imgNode = document.createElement('img');
    imgNode.src = FileUrl;
    imgNode.className = "img-fluid";
    $('#content').summernote('insertNode', imgNode);
}





function showImage(fileUrl) {
    addImage(fileUrl, '#div_img');
}

function showImages(fileUrl) {
    addImage(fileUrl, '#div_imgs')
}

function showDocument(fileUrl) {
    $('#div_doc').html("<div>Файл загружен. Название файла: " + fileUrl + " " + "<button class='btn btn-light' onclick='requestRemoveFile(this, \"" + fileUrl + "\")'>Удалить</button></div>");
    $('#hidden_doc').html('<input id="doc_path" name="doc_path" type="hidden" value="' + fileUrl + '" />');

}

function addImage(fileUrl, id) {

    var imgNode = "<div><img src='" + fileUrl + "' class='img_view'><br/><button class='btn btn_delete' onclick='requestRemoveFile(this, \"" + fileUrl + "\")'>&#215;</button></div>";

    if (id == '#div_img') {
        $(id).html(imgNode);
        $('#hidden_img').html('<input id="img_path" name="img_path" type="hidden" value="' + fileUrl + '" />');
    }
    else {
        $(id).append(imgNode);
    }
}





$('#btnSubmitProject').click(function () {
    addDataToHiddenImgs();
})

function addDataToHiddenImgs() {
    var div_imgs = document.querySelector("#div_imgs");
    var img_nodes = div_imgs.childNodes;

    let img, str, searchStr, from, to, fileUrl;

    searchStr = '/Content/Upload';

    for (let i = 0; i < img_nodes.length; i++) {
        img = img_nodes[i].childNodes;

        str = img[0].src;
        from = str.search(searchStr);
        to = str.length;
        fileUrl = str.substring(from, to);

        $('#hidden_imgs').append('<input class="hidden_imgs" id="images_' + i + '__img_path" name="images[' + i + '].img_path" type="hidden" value="' + fileUrl + '" />');

    }
}