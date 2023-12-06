Admin_products = new Vue({
    el: '#Admin_products',
    data: {
        dataItems: [],
        dataProductsItems: [],
        dataSubItems: [],
        filteredSubCategories: [],
        dataBrands: [],
        dataAge: [],
        dataItemsRim: [],
        dataCLB: [],
        dataSize: [],
        dataColor: [],

        id: "",
        CategoryID: 0,
        SubCategoryID: 0,
        AgeID: 0,
        RimID: 0,
        brandsID: 0,
        clbID: 0,
        sizeID: 0,
        SizeArray: [],

        ProductName: "",
        ProductNumber: "",
        BeforePrice: 0,
        DisPrice: 0,
        Quantity: 0,
        Material: "",
        colorID: 0,

        imageFile: null,
        previewImage: null,
        uploadedImage: null,
        imageProducts: "",
        SubCategoryName: "",
        Slug: "",

        selectedFiles: null,
        imagesPreview: [],
        fullImg: [],
        processedFiles: [],
        listNumber: [],
        suggestions: [],
        originalDataColor :[]

     

    },
    computed: {
        computedSlug() {
            return this.generateSlug(this.ProductName);
        }
        
    },
    watch: {
        computedSlug(newVal) {
            this.Slug = newVal;
        },
        CategoryID(newVal) {
            this.filteredSubCategories = this.dataSubItems.filter(subCate => subCate.cateId === newVal);
        },
        processedFiles: {
            handler(newValue) {
                if (newValue && newValue.length) {
                    for (let i = 0; i < newValue.length; i++) {
                        console.log(newValue[i]);
                    }
                }
            },
            deep: true
        },
        ProductNumber: function (newVal, oldVal) {
            this.updateAvailableColors();
        }
    },
    mounted() {
        
        $('#preloader').fadeIn();
        axios.get("/Products/GetAllCategory")
            .then((response) => {
                this.dataItems = response.data;
                return Promise.resolve();
            });
        axios.get("/Products/GetAllSubCategory")
            .then((response) => {
                this.dataSubItems = response.data;
                return Promise.resolve();
            })
        axios.get("/Products/GetAllBrands")
            .then((response) => {
                this.dataBrands = response.data;
                return Promise.resolve();
            })
        axios.get("/Config/GetAllAge")
            .then((response) => {
                this.dataAge = response.data;
                return Promise.resolve();
            })
        axios.get("/Config/GetAllRim")
            .then((response) => {
                this.dataItemsRim = response.data;
                return Promise.resolve();
            })
        axios.get("/Config/GetAllSize")
            .then((response) => {
                this.dataSize = response.data;
                return Promise.resolve();
            })
        axios.get("/Products/GetAllCLB")
            .then((response) => {
                this.dataCLB = response.data;
                return Promise.resolve();
            })
        axios.get("/Config/GetAllColor")
            .then((response) => {
                this.dataColor = response.data;
                this.originalDataColor = [...this.dataColor]; 
                return Promise.resolve();
            })
        this.loadSubItems();

    },
    methods: {
        updateAvailableColors() {
            if (this.ProductNumber == "") {
                this.dataColor = this.originalDataColor;
                return;
            }

            const selectedProduct = this.dataProductsItems.find(item => item.productNumber === this.ProductNumber);

            if (selectedProduct) {
                this.dataColor = this.originalDataColor.filter(color => color.id !== selectedProduct.colorID);
            }
        },
        onInput() {
            if (this.ProductNumber) {
                this.suggestions = this.listNumber.filter(
                    item => item.productNumber.includes(this.ProductNumber)
                );
            } else {
                this.suggestions = [];
            }
        },

        selectSuggestion(suggestion) {
            this.ProductNumber = suggestion.productNumber; 
            this.suggestions = [];
            this.updateAvailableColors();
        },

        updateCateJson: function () {
            this.SizeArray = this.dataSize
                .filter(item => item.selected)
                .map(item => item.id);
        },
        removeImage(index) {
            // Xóa ảnh từ mảng imagesPreview
            this.imagesPreview.splice(index, 1);

            // Xóa tệp từ mảng processedFiles
            if (this.processedFiles && this.processedFiles.length > index) {
                this.processedFiles.splice(index, 1);
                console.log(this.processedFiles);
            }
        },
        formatCurrency(amount) {
            const formatter = new Intl.NumberFormat('vi-VN', {
                style: 'currency',
                currency: 'VND'
            });

            return formatter.format(amount);
        },
        loadSubItems() {
            let currentPage = 0;
            if ($.fn.DataTable.isDataTable('#product_table')) {
                currentPage = $('#product_table').DataTable().page();
                $('#product_table').DataTable().destroy();
            }
            axios.get("/Products/GetAllProduct")
                .then((response) => {
                    this.dataProductsItems = response.data;
                    this.listNumber = this.dataProductsItems.map(item => ({
                        productNumber: item.productNumber,
                        nameProduct: item.nameProduct
                    }));
                    console.log(" this.listNumber", this.listNumber)
                    $('#preloader').fadeOut();

                    return Promise.resolve();
                })
                .then(() => {
                    const table = $("#product_table").DataTable({
                        'columnDefs': [{
                            'targets': [-1],
                            'orderable': false,
                        }],
                        searching: true,
                        iDisplayLength: 7,
                        "ordering": false,
                        lengthChange: false,
                        aaSorting: [[0, "desc"]],
                        aLengthMenu: [
                            [5, 10, 25, 50, 100, -1],

                            ["5 dòng", "10 dòng", "25 dòng", "50 dòng", "100 dòng", "Tất cả"],
                        ],
                        language: {
                            "processing": "Đang xử lý...",
                            "aria": {
                                "sortAscending": ": Sắp xếp thứ tự tăng dần",
                                "sortDescending": ": Sắp xếp thứ tự giảm dần"
                            },
                            "autoFill": {
                                "cancel": "Hủy",
                                "fill": "Điền tất cả ô với <i>%d<\/i>",
                                "fillHorizontal": "Điền theo hàng ngang",
                                "fillVertical": "Điền theo hàng dọc"
                            },
                            "buttons": {
                                "collection": "Chọn lọc <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                                "colvis": "Hiển thị theo cột",
                                "colvisRestore": "Khôi phục hiển thị",
                                "copy": "Sao chép",
                                "copyKeys": "Nhấn Ctrl hoặc u2318 + C để sao chép bảng dữ liệu vào clipboard.<br \/><br \/>Để hủy, click vào thông báo này hoặc nhấn ESC",
                                "copySuccess": {
                                    "1": "Đã sao chép 1 dòng dữ liệu vào clipboard",
                                    "_": "Đã sao chép %d dòng vào clipboard"
                                },
                                "copyTitle": "Sao chép vào clipboard",
                                "pageLength": {
                                    "-1": "Xem tất cả các dòng",
                                    "_": "Hiển thị %d dòng",
                                    "1": "Hiển thị 1 dòng"
                                },
                                "print": "In ấn",
                                "createState": "Tạo trạng thái",
                                "csv": "CSV",
                                "excel": "Excel",
                                "pdf": "PDF",
                                "removeAllStates": "Xóa hết trạng thái",
                                "removeState": "Xóa",
                                "renameState": "Đổi tên",
                                "savedStates": "Trạng thái đã lưu",
                                "stateRestore": "Trạng thái %d",
                                "updateState": "Cập nhật"
                            },
                            "select": {
                                "cells": {
                                    "1": "1 ô đang được chọn",
                                    "_": "%d ô đang được chọn"
                                },
                                "columns": {
                                    "1": "1 cột đang được chọn",
                                    "_": "%d cột đang được được chọn"
                                },
                                "rows": {
                                    "1": "1 dòng đang được chọn",
                                    "_": "%d dòng đang được chọn"
                                }
                            },
                            "searchBuilder": {
                                "title": {
                                    "_": "Thiết lập tìm kiếm (%d)",
                                    "0": "Thiết lập tìm kiếm"
                                },
                                "button": {
                                    "0": "Thiết lập tìm kiếm",
                                    "_": "Thiết lập tìm kiếm (%d)"
                                },
                                "value": "Giá trị",
                                "clearAll": "Xóa hết",
                                "condition": "Điều kiện",
                                "conditions": {
                                    "date": {
                                        "after": "Sau",
                                        "before": "Trước",
                                        "between": "Nằm giữa",
                                        "empty": "Rỗng",
                                        "equals": "Bằng với",
                                        "not": "Không phải",
                                        "notBetween": "Không nằm giữa",
                                        "notEmpty": "Không rỗng"
                                    },
                                    "number": {
                                        "between": "Nằm giữa",
                                        "empty": "Rỗng",
                                        "equals": "Bằng với",
                                        "gt": "Lớn hơn",
                                        "gte": "Lớn hơn hoặc bằng",
                                        "lt": "Nhỏ hơn",
                                        "lte": "Nhỏ hơn hoặc bằng",
                                        "not": "Không phải",
                                        "notBetween": "Không nằm giữa",
                                        "notEmpty": "Không rỗng"
                                    },
                                    "string": {
                                        "contains": "Chứa",
                                        "empty": "Rỗng",
                                        "endsWith": "Kết thúc bằng",
                                        "equals": "Bằng",
                                        "not": "Không phải",
                                        "notEmpty": "Không rỗng",
                                        "startsWith": "Bắt đầu với",
                                        "notContains": "Không chứa",
                                        "notEndsWith": "Không kết thúc với",
                                        "notStartsWith": "Không bắt đầu với"
                                    },
                                    "array": {
                                        "equals": "Bằng",
                                        "empty": "Trống",
                                        "contains": "Chứa",
                                        "not": "Không",
                                        "notEmpty": "Không được rỗng",
                                        "without": "không chứa"
                                    }
                                },
                                "logicAnd": "Và",
                                "logicOr": "Hoặc",
                                "add": "Thêm điều kiện",
                                "data": "Dữ liệu",
                                "deleteTitle": "Xóa quy tắc lọc",
                                "leftTitle": "Giảm thụt lề",
                                "rightTitle": "Tăng thụt lề"
                            },
                            "searchPanes": {
                                "countFiltered": "{shown} ({total})",
                                "emptyPanes": "Không có phần tìm kiếm",
                                "clearMessage": "Xóa hết",
                                "loadMessage": "Đang load phần tìm kiếm",
                                "collapse": {
                                    "0": "Phần tìm kiếm",
                                    "_": "Phần tìm kiếm (%d)"
                                },
                                "title": "Bộ lọc đang hoạt động - %d",
                                "count": "{total}",
                                "collapseMessage": "Thu gọn tất cả",
                                "showMessage": "Hiện tất cả"
                            },
                            "datetime": {
                                "hours": "Giờ",
                                "minutes": "Phút",
                                "next": "Sau",
                                "previous": "Trước",
                                "seconds": "Giây",
                                "amPm": [
                                    "am",
                                    "pm"
                                ],
                                "unknown": "-",
                                "weekdays": [
                                    "Chủ nhật"
                                ],
                                "months": [
                                    "Tháng Một",
                                    "Tháng Hai",
                                    "Tháng Ba",
                                    "Tháng Tư",
                                    "Tháng Năm",
                                    "Tháng Sáu",
                                    "Tháng Bảy",
                                    "Tháng Tám",
                                    "Tháng Chín",
                                    "Tháng Mười",
                                    "Tháng Mười Một",
                                    "Tháng Mười Hai"
                                ]
                            },
                            "emptyTable": "Không có dữ liệu",
                            "info": "Hiển thị _START_ tới _END_ của _TOTAL_ dữ liệu",
                            "infoEmpty": "Hiển thị 0 tới 0 của 0 dữ liệu",
                            "lengthMenu": "Hiển thị _MENU_ dữ liệu",
                            "loadingRecords": "Đang tải...",
                            "paginate": {
                                "first": "Đầu tiên",
                                "last": "Cuối cùng",
                                "next": "Sau",
                                "previous": "Trước"
                            },
                            "search": "Tìm kiếm:",
                            "zeroRecords": "Không tìm thấy kết quả",
                            "decimal": ",",
                            "editor": {
                                "close": "Đóng",
                                "create": {
                                    "button": "Thêm",
                                    "submit": "Thêm",
                                    "title": "Thêm mục mới"
                                },
                                "edit": {
                                    "button": "Sửa",
                                    "submit": "Cập nhật",
                                    "title": "Sửa mục"
                                },
                                "error": {
                                    "system": "Đã xảy ra lỗi hệ thống (&lt;a target=\"\\\" rel=\"nofollow\" href=\"\\\"&gt;Thêm thông tin&lt;\/a&gt;)."
                                },
                                "multi": {
                                    "info": "Các mục đã chọn chứa các giá trị khác nhau cho đầu vào này. Để chỉnh sửa và đặt tất cả các mục cho đầu vào này thành cùng một giá trị, hãy nhấp hoặc nhấn vào đây, nếu không chúng sẽ giữ lại các giá trị riêng lẻ của chúng.",
                                    "noMulti": "Đầu vào này có thể được chỉnh sửa riêng lẻ, nhưng không phải là một phần của một nhóm.",
                                    "restore": "Hoàn tác thay đổi",
                                    "title": "Nhiều giá trị"
                                },
                                "remove": {
                                    "button": "Xóa",
                                    "confirm": {
                                        "_": "Bạn có chắc chắn muốn xóa %d hàng không?",
                                        "1": "Bạn có chắc chắn muốn xóa 1 hàng không?"
                                    },
                                    "submit": "Xóa",
                                    "title": "Xóa"
                                }
                            },
                            "infoFiltered": "(được lọc từ _MAX_ dữ liệu)",
                            "searchPlaceholder": "Nhập tìm kiếm...",
                            "stateRestore": {
                                "creationModal": {
                                    "button": "Thêm",
                                    "columns": {
                                        "search": "Tìm kiếm cột",
                                        "visible": "Khả năng hiển thị cột"
                                    },
                                    "name": "Tên:",
                                    "order": "Sắp xếp",
                                    "paging": "Phân trang",
                                    "scroller": "Cuộn vị trí",
                                    "search": "Tìm kiếm",
                                    "searchBuilder": "Trình tạo tìm kiếm",
                                    "select": "Chọn",
                                    "title": "Thêm trạng thái",
                                    "toggleLabel": "Bao gồm:"
                                },
                                "duplicateError": "Trạng thái có tên này đã tồn tại.",
                                "emptyError": "Tên không được để trống.",
                                "emptyStates": "Không có trạng thái đã lưu",
                                "removeConfirm": "Bạn có chắc chắn muốn xóa %s không?",
                                "removeError": "Không xóa được trạng thái.",
                                "removeJoiner": "và",
                                "removeSubmit": "Xóa",
                                "removeTitle": "Xóa trạng thái",
                                "renameButton": "Đổi tên",
                                "renameLabel": "Tên mới cho %s:",
                                "renameTitle": "Đổi tên trạng thái"
                            },
                            "infoThousands": ".",
                            "thousands": "."
                        },
                    });
                    if (currentPage !== 0) {
                        table.page(currentPage).draw('page');
                    }
                });
        },
        generateSlug(text) {
            // Loại bỏ các dấu tiếng Việt
            let str = text.normalize('NFD').replace(/[\u0300-\u036f]/g, '');

            // Chuyển đổi sang chữ thường và thay thế các ký tự không phải chữ và số
            return str
                .toLowerCase()
                .replace(/ /g, '-')
                .replace(/[^\w-]+/g, '');
        },
        previewFiles(event) {
            const newFilesArray = event.target.files; // Chuyển FileList thành mảng

            // Nối mảng tệp mới với mảng tệp hiện tại
            this.processedFiles = newFilesArray;

            // Thêm ảnh xem trước cho tệp mới
            for (let i = 0; i < newFilesArray.length; i++) {
                const imgSrc = URL.createObjectURL(newFilesArray[i]);
                this.imagesPreview.push(imgSrc);
            }
        },


        onFileChange(event) {
            this.imageFile = event.target.files[0];
            this.previewImage = URL.createObjectURL(this.imageFile);
            this.uploadedImage = null;
        },
        formatDate(date) {
            const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
            return date.toLocaleDateString('vi-VN', options);
        },
        formatCurrency(amount) {
            const formatter = new Intl.NumberFormat('vi-VN', {
                style: 'currency',
                currency: 'VND'
            });

            return formatter.format(amount);
        },
        async addProducts() {
            try {
                if (this.DisPrice > this.BeforePrice) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Giá giảm phải nhỏ hơn giá chính',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                if (this.Quantity == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Vui lòng nhập số lượng',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                if (this.CategoryID === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Vui lòng nhập chọn danh mục',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                if (this.SubCategoryID === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Vui lòng nhập chọn danh mục phụ',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                const formData = new FormData();
                formData.append('NameProduct', this.ProductName);
                formData.append('ProductNumber', this.ProductNumber);
                formData.append('BeforePrice', this.BeforePrice);
                formData.append('DisPrice', this.DisPrice);
                formData.append('CateId', this.CategoryID);
                formData.append('SubId', this.SubCategoryID);
                formData.append('Quantity', this.Quantity);
                formData.append('BrandId', this.brandsID);
                formData.append('CLBID', this.clbID);
                formData.append('AgeID', this.AgeID);
                formData.append('RimID', this.RimID);
                formData.append('Slug', this.Slug);
                formData.append('ColorID', this.colorID);
                formData.append('Material', this.Material);
                for (let i = 0; i < this.SizeArray.length; i++) {
                    formData.append('SizeFull', this.SizeArray[i]);
                }

                formData.append('PrPath', this.$refs.PrPath.files[0]);
                for (let i = 0; i < this.processedFiles.length; i++) {
                    formData.append('PrPathFull', this.processedFiles[i]);
                }

                await axios.post('/Products/AddProducts', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã có lỗi xảy ra',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            }
        },
        getItemsById(id) {
            axios.get(`/Products/getIdProducts?id=${id}`)
                .then((response) => {
                   
                    this.id = response.data.id;
                    this.CategoryID = response.data.cateId;
                    this.SubCategoryID = response.data.subId;
                    this.AgeID = response.data.ageID;
                    this.RimID = response.data.rimID;
                    this.brandsID = response.data.brandId;
                    this.clbID = response.data.clbid;
                    this.Slug = response.data.slug;
                    this.ProductName = response.data.nameProduct;
                    this.ProductNumber = response.data.productNumber;
                    this.BeforePrice = response.data.beforePrice;
                    this.DisPrice = response.data.disPrice;
                    this.Quantity = response.data.quantity;
                    this.imageProducts = response.data.imgProducts;
                    this.colorID = response.data.colorID;
                    this.Material = response.data.material;
                    this.fullImg = JSON.parse(response.data.imgFull);
                    this.selectedFiles = this.fullImg;
                    this.imagesPreview = this.fullImg;
                    this.SizeArray = JSON.parse(response.data.sizeId).map(Number);

                    this.dataSize.forEach(item => {
                        item.selected = this.SizeArray.includes(item.id);
                    });
                   
                    return Promise.resolve();
                });
        },
        resetData() {
            this.id = "";
            this.CategoryID =0;
            this.SubCategoryID = 0;
            this.AgeID = 0;
            this.RimID = 0;
            this.brandsID = 0;
            this.clbID = 0;
            this.Slug = "";
            this.ProductName = "";
            this.ProductNumber = "";
            this.BeforePrice = 0;
            this.DisPrice =0;
            this.Quantity = 0;
            this.imageProducts = "";
            this.previewImage = null;
            this.selectedFiles = null;
            this.processedFiles = [];
            this.imagesPreview = [];
            this.fullImg = [];
            this.SizeArray = [];
            this.$refs.PrPath1.files[0] = null;
            this.$refs.PrPath.files[0] = null;
            this.$refs.fileInput.value = ''; 
            this.colorID =0;
            this.Material = "";
        },
        async editProducts() {
            try {
                if (this.BeforePrice < this.DisPrice) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Giá giảm phải nhỏ hơn giá chính',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                if (this.Quantity == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Vui lòng nhập số lượng',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                if (this.CategoryID === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Vui lòng nhập chọn danh mục',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                if (this.SubCategoryID === 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Vui lòng nhập chọn danh mục phụ',
                        confirmButtonText: 'OK'
                    })
                    return;
                }
                const formData = new FormData();
                formData.append('ID', this.id);
                formData.append('NameProduct', this.ProductName);
                formData.append('ProductNumber', this.ProductNumber);
                formData.append('BeforePrice', this.BeforePrice);
                formData.append('DisPrice', this.DisPrice);
                formData.append('CateId', this.CategoryID);
                formData.append('SubId', this.SubCategoryID);
                formData.append('Quantity', this.Quantity);
                formData.append('BrandId', this.brandsID);
                formData.append('CLBID', this.clbID);
                formData.append('AgeID', this.AgeID);
                formData.append('RimID', this.RimID);
                formData.append('Slug', this.Slug);
                formData.append('ColorID', this.colorID);
                formData.append('Material', this.Material);
                if (this.$refs.PrPath1.files[0] != null) {

                    formData.append('PrPath', this.$refs.PrPath1.files[0]);
                }
                if (this.processedFiles != null) {
                    for (let i = 0; i < this.processedFiles.length; i++) {
                        formData.append('PrPathFull', this.processedFiles[i]);
                    }
                }
                if (this.processedFiles.length === 0)
                {

                    if (this.imagesPreview != null) {
                        this.processedFiles = this.imagesPreview;
                        for (let i = 0; i < this.processedFiles.length; i++) {
                            formData.append('PrPathFull', this.processedFiles[i]);
                        }
                    }
                    else {
                        formData.append('PrPathFullEmpty', 'true');

                    }

                }
                for (let i = 0; i < this.SizeArray.length; i++) {
                    formData.append('SizeFull', this.SizeArray[i]);
                }
               
                await axios.post('/Products/UpdateProducts', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        this.loadSubItems();
                    }
                });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Đã có lỗi xảy ra',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();

                    }
                });
            }
        },
        getItemsByIdDelete(id) {
            axios.get(`/Products/getIdProducts/${id}`)
                .then((response) => {
                    this.id = response.data.id;
                    if (this.id != null) {
                        Swal.fire({
                            title: 'Xóa sản phẩm',
                            text: 'Bạn có chắc chắn muốn xóa',
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonText: 'Đồng ý',
                            cancelButtonText: 'Không!!!'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                const formData = new FormData();
                                formData.append('ID', this.id);
                                axios.post('/Products/DeleteProducts', formData, {
                                    headers: {
                                        'Content-Type': 'application/x-www-form-urlencoded'
                                    }
                                }).then(response => {
                                    Swal.fire({
                                        icon: 'success',
                                        title: 'Thành công',
                                        text: 'Đã thành công',
                                        confirmButtonText: 'OK',
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            this.loadSubItems();

                                        }
                                    });

                                }).catch(error => {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Lỗi',
                                        text: 'Đã có lỗi xảy ra vui lòng thử lại',
                                        confirmButtonText: 'OK'
                                    });
                                });
                            } else {
                                return;
                            }
                        });
                    }
                }).catch((error) => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: 'Đã có lỗi xảy ra vui lòng thử lại',
                        confirmButtonText: 'OK'
                    });
                })
        },
    }
})