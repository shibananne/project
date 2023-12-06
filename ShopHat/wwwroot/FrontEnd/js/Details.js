var details_vue = new Vue({
    el: '#details_vue',
    data: {
        dataBrand: [],
        dataClb: [],
        dataSub: [],
        dataProducts: [],
        dataSize: [],
        dataProductColor: [],


        CountBrand: "",
        CountClb: "",
        product_number: "",
        CountProductsHeader: 0,

        viewedProducts: 0,

        pageNumber: 1,
        pageSize: 4,
        brandsId: 0,

        dataHeart: [],
        QuantityClient: 1,
        productId: null,
        sizeId: null,
        jsonSize: [],
        canLoadMore: false,
        CountProducts: 0,
        SelectSize: "Chọn size",
        currentColorName: '',
    },
    computed: {
        filteredSizes() {

            let ids = this.jsonSize.map(id => parseInt(id));
            return this.dataSize.filter(size => ids.includes(size.id));
        }
    },

    created: function () {
        EventBus.$on('countProductsChanged', (value) => {
            this.CountProductsHeader = value;
        });

        EventBus.$on('dataProductsHeart', (value) => {
            this.dataHeart = value;
        });
    },
    watch: {
        SelectSize(valuePr){
            console.log(valuePr);
        }
    },
    mounted() {
        $('#preloader').fadeIn();

        this.brandsId = $("#brandsId").val();
        this.productId = $("#productId").val();
        this.product_number = $("#product_number").val();
        this.currentColorName = $("#colorNameId").val();
        console.log(this.product_number);

        let jsonString = $("#sizeId").val();
        try {
            if (jsonString != null) {

                this.jsonSize = JSON.parse(jsonString);
            }
        } catch (e) {
            console.error("Failed to parse the JSON string: ", e);
        }
        if (this.product_number != "") {
            const formData = new FormData();
            formData.append('num', this.product_number);
            axios.post('/Details/DetailsProductsColor', formData,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
           
                .then((response) => {
                    this.dataProductColor = response.data.products;
                    return Promise.resolve();
                })
        }
        axios.get(`/Details/brands/${this.brandsId}`, {
            params: {
                pageNumber: this.pageNumber,
                pageSize: this.pageSize,
                excludeProductId: this.productId
            }
        })
            .then((response) => {
                this.dataProducts = response.data.products;
                this.CountProducts = response.data.count;
                return Promise.resolve();
            })
        axios.get("/Config/GetAllSize")
            .then((response) => {
                this.dataSize = response.data;
                return Promise.resolve();
            })
        $('#preloader').fadeOut();

    },
    methods: {
        redirect() {
            const slug = $("#product_slug").val();
            location.href = `/Category/${slug}`;
        },
        async dataHeartCart() {
            const userCookie = this.getCookie("UniqueUserIdentifier");

            try {
                const formData = new FormData();
                formData.append('QuantityClient', this.QuantityClient);
                formData.append('UserCookie', userCookie);
                formData.append('ProductID', this.productId);
                formData.append('sizeId', this.SelectSize);

                await axios.post('/Details/AddCart', formData,
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
                        header_vue.getCart();
                        header_vue.openCart();
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
        getCookie(name) {
            const value = `; ${document.cookie}`;
            const parts = value.split(`; ${name}=`);
            if (parts.length === 2) return parts.pop().split(';').shift();
        },
        loadMore() {
            if (!this.canLoadMore) return;
            this.viewedProducts += 1;
            if (this.viewedProducts > this.CountProducts) {
                this.canLoadMore = false;
            } else {
                this.updateProgress();
            }

        },

        updateProgress() {
            if (this.CountProducts === 0) {
                this.pageNumber = 0;
                return;
            }
            if (this.CountProducts > 0) {
                this.pageNumber++;
            }
            $('#preloader').fadeIn();

            axios.get(`/Details/brands/${this.brandsId}`, {
                params: {
                    pageNumber: this.pageNumber,
                    pageSize: this.pageSize,
                    excludeProductId: this.productId
                }
            })
                .then((response) => {
                    this.dataProducts = [...this.dataProducts, ...response.data.products];
                    $('#preloader').fadeOut();
                    let widthPercentage = (this.viewedProducts / this.CountProductsHeader) * 100;
                    document.querySelector('.bellow-line').style.width = widthPercentage + "%";
                    document.querySelector('.viewed-text').innerText = `Bạn đã xem ${this.viewedProducts}/${this.CountProducts} sản phẩm`;

                });

        },
        formatPrice(value) {
            if (!value) return '0 VND';

            return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") + ' VND';
        },
    }
});