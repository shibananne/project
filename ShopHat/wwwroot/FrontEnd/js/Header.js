var header_vue = new Vue({
    el: '#header_vue',
    data: {
        dataBrand: [],
        dataBrandNews: [],
        dataClb: [],
        dataSub: [],
        dataProducts: [],


        CountBrand: "",
        CountClb: "",
        CountProductsHeader: "",
        CountBrandCus: "",


        viewedProducts: 1,

        pageNumber: 1,
        pageSize: 1,
        Heart: [],
        selected: 'main',

        count: 0,

        CartAdd: [],
        cartOpen: false,
        quantity: 0,

        orderId: 0,
        canLoadMore: true,

        isFixed: false, 
        isIconVisible: false,
        isNav: false,
        isIcon: false,


        dataCate: [],
        dataCate2: [],
        dataCate3: [],
        dataClbMain: [],
        dataClb: [],
        activeMainClb: "Motor",
        isFirstLoad: true,

        dataBrandNews: [],
        categories: [
            { id: 1, name: 'Mũ lưỡi trai', apiEndpoint: '/Home/GetAllSubCategoryWithId/1' },
            { id: 2, name: 'Mũ beanie', apiEndpoint: '/Home/GetAllSubCategoryWithId/2' },
            { id: 3, name: 'Mũ ', apiEndpoint: '/Home/GetAllSubCategoryWithId/2' },

        ],
        categoryData: {},
        hoverCategoryId: null,
        isNav: false,
        AboutMain:"",
        Terms:"",
        AboutPage:"",
        ReturnPr:"",

    },
    created: function () {
        //EventBus.$on('countProductsChanged', (value) => {
        //    this.CountProductsHeader = value;
        //});
        //EventBus.$on('dataProducts', (value) => {
        //    this.dataProducts = value;
        //});
        EventBus.$on('dataProductsHeart', (value) => {
            this.Heart = value;
        });
        EventBus.$on('dataSeclect', (value) => {
            this.selected = value;
        });
        //const savedSelection = localStorage.getItem('selectedSection');
        //if (savedSelection) {
           
        //};
        const storedData = localStorage.getItem('selectedSection');
        if (storedData) {
            const { section, expirationTime } = JSON.parse(storedData);

            if (new Date().getTime() > expirationTime) {
                localStorage.removeItem('selectedSection');
            } else {
                this.selected = section;
            }
        }
       
    },
    computed: {
        totalPrices() {
            let totalPrice = 0;
            if (this.CartAdd && this.CartAdd.length > 0) {
                this.CartAdd.forEach(item => {
                    totalPrice += item.totalPrices;
                });
            }
          
            return totalPrice;
        },
       
    },
    watch: {
        CartAdd(valuePr) {
            EventBus.$emit('dataCart', valuePr);
        },
        orderId(value) {
            EventBus.$emit('orderId', value);

        },
        dataBrandNews(valuePr) {
            EventBus.$emit('dataBrands', valuePr);
        } ,
        isNav(valuePr) {
            EventBus.$emit('isNavClick', valuePr);

        },
        AboutMain(val) {
            EventBus.$emit('MainPage', val);
        },
        Terms(val) {
            EventBus.$emit('TermsPage', val);
        },
        AboutPage(val) {
            EventBus.$emit('AboutPages', val);
        },
        ReturnPr(val) {
            EventBus.$emit('ReturnPrPage', val);
        }
    },
    mounted() {
        window.addEventListener('scroll', this.handleScroll);
        axios.get("/Home/GetAllBrands")
            .then((response) => {
                this.dataBrandNews = response.data.brands;
                this.CountBrandCus = 7;

                return Promise.resolve();
            })
        this.getCart();
        axios.get(`/Home/GetAllProduct?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`)
            .then((response) => {
                this.dataProducts = response.data.products;
                this.CountProductsHeader = response.data.count;
                return Promise.resolve();
            })
        axios.get(`/Home/GetAllSubCategoryWithId/1`)
            .then((response) => {
                this.dataCate = response.data.subCategories;
                return Promise.resolve();
            })
        axios.get(`/Home/GetAllSubCategoryWithId/2`)
            .then((response) => {
                this.dataCate2 = response.data.subCategories;
                return Promise.resolve();
            })
        axios.get(`/Home/GetAllSubCategoryWithId/3`)
            .then((response) => {
                this.dataCate3 = response.data.subCategories;
                return Promise.resolve();
            })
        this.categories.forEach(category => {
            axios.get(category.apiEndpoint)
                .then(response => {
                    this.$set(this.categoryData, category.id, response.data.subCategories);
                });
        });
        axios.get(`/Home/GetAllClbMain`)
            .then((response) => {
                this.dataClbMain = response.data.clb;
                return Promise.resolve();
            })
        axios.get(`/Home/GetAllClbWithMain`)
            .then((response) => {
                this.dataClb = response.data.clBs;
                this.activeMainClb = this.dataClb[0];

                return Promise.resolve();
            })
        axios.get(`/Config/GetTermsPr`)
            .then((response) => {
                this.AboutMain = response.data.aboutMain;
                this.Terms = response.data.terms;
                this.AboutPage = response.data.aboutPage;
                this.ReturnPr = response.data.refunPr;
                return Promise.resolve();
            })
    },
    beforeDestroy() {
        window.removeEventListener('scroll', this.handleScroll);
    },
    methods: {
        mouseOver(id) {
            this.activeMainClb = id;
            this.isFirstLoad = false;
        },
        mouseOverCate(hoverCategoryId) {
            this.hoverCategoryId = hoverCategoryId;
        },
        categoryDetails(val) {
            location.href = `/Teams/${val.slug}`
        },
        categoryDetailsTeam(val) {
            location.href = `/Teams/${val.slug}`

        },
        navClick() {
            this.isNav = !this.isNav;
        },
        handleScroll() {
            const offset = 100; 
            this.isFixed = window.scrollY >= offset; 
            this.isIconVisible = window.scrollY >= offset
            if (window.scrollY === 0) {
                this.isIcon = false;
                this.isNav = false;
            } else {
                this.isIcon = true;
                this.isNav = true;


            }
        },
        getDiscount(product) {
            if (product.beforePrice && product.disPrice) {
                let discount = (1 - (product.disPrice / product.beforePrice)) * 100;
                return `-${Math.floor(discount)}%`;
            }
            return '0%';
        },
        homeSrceen() {
            const storedData = localStorage.getItem('selectedSection');
            if (storedData) {
                localStorage.removeItem('selectedSection');
            }
            location.href = "/";
        },
        toggleCart(event) {
            event.preventDefault();
            this.cartOpen = !this.cartOpen;
            if (this.cartOpen) {
                this.openCart();
                this.$el.addEventListener('click', this.closeCartOnClickOutside);
            } else {
                this.closeCart();
                this.$el.removeEventListener('click', this.closeCartOnClickOutside);
            }
        },
        openCart() {
            this.cartOpen = true;
            this.$el.addEventListener('click', this.closeCartOnClickOutside);

            $('body').addClass('open');
        },
        closeCart() {
            this.cartOpen = false;
            $('body').removeClass('open');
        },
        closeCartOnClickOutside(event) {
            if (this.cartOpen) {
                let cartAsideExists = this.$refs.cartAside && this.$refs.cartAside.contains(event.target);
                let toggleCartExists = this.$refs.toggleCart && this.$refs.toggleCart.contains(event.target);

                if (!cartAsideExists && !toggleCartExists) {
                    this.cartOpen = false;
                    this.closeCart();
                    document.removeEventListener('click', this.closeCartOnClickOutside);
                }
            }
        },
        loadMore() {
            if (!this.canLoadMore) return; 
            this.viewedProducts += 1;
            if (this.viewedProducts > this.CountProductsHeader) {
                this.canLoadMore = false; 
            } else {
                this.updateProgress();
            }
        }, 
        selectSection(section) {
            this.selected = section;
            const expirationTime = new Date().getTime() + 3600 * 1000; 
            localStorage.setItem('selectedSection', JSON.stringify({ section, expirationTime }));

            window.location.reload();
        },
        checkoutLocation() {
            location.href = "/Checkout";
        },
        getCart() {
            axios.get("/Home/GetAllCart")
                .then((response) => {
                    this.CartAdd = response.data.products;
                    this.orderId = response.data.orderId;
                    return Promise.resolve();
                })
            axios.get("/Home/GetCountCart")
                .then((response) => {
                    this.count = response.data.count;
                    return Promise.resolve();
                })
        },
        GetCount() {
            axios.get("/Home/GetCountCart")
                .then((response) => {
                    this.count = response.data.count;
                    return Promise.resolve();
                })
        },
        updateProgress() {
            $('#preloader').fadeIn();

            this.pageNumber++;
            axios.get(`/Home/GetAllProduct?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`)
                .then((response) => {
                    this.dataProducts = [...this.dataProducts, ...response.data.products];
                    $('#preloader').fadeOut();
                    let widthPercentage = (this.viewedProducts / this.CountProductsHeader) * 100;
                    document.querySelector('.bellow-line').style.width = widthPercentage + "%";
                    document.querySelector('.viewed-text').innerText = `Bạn đã xem ${this.viewedProducts}/${this.CountProductsHeader} sản phẩm`;

                });
        },
        formatCurrency(amount) {
            const formatter = new Intl.NumberFormat('vi-VN', {
                style: 'currency',
                currency: 'VND'
            });

            return formatter.format(amount);
        },
        formatPrice(value) {
            if (!value) return '0 VND';

            return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") + ' VND';
        },
        async deleteCart(product) {
            try {
                $('#preloader-2').fadeIn();

                const formData = new FormData();

                formData.append('idOrder', product.orderId);
                formData.append('prId', product.id);
                formData.append('OrderDetails', product.idDetails);

                await axios.post('/Home/DeleteCart', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                $('#preloader-2').fadeOut();

                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        this.getCart();
                    }
                });
            }
            catch (error) {
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
        async increaseQuantity(cartOrder) {
            cartOrder.quantityClient += 1;
            try {
                await this.updateQuantity(cartOrder, "increase");
            } catch (error) {
                console.error(error);
                cartOrder.quantityClient -= 1;
            }
        },
        async decreaseQuantity(cartOrder) {
            cartOrder.quantityClient -= 1;
            try {
                await this.updateQuantity(cartOrder, "decrease");
            } catch (error) {
                console.error(error);
                cartOrder.quantityClient += 1;
            }
        },
        async updateQuantity(product, action) {
            try {
                $('#preloader-2').fadeIn();

                const formData = new FormData();

                formData.append('action', action);
                formData.append('prId', product.id);
                formData.append('OrderDetails', product.idDetails);

                await axios.post('/Home/UpdateCart', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                $('#preloader-2').fadeOut();

                this.getCart();

            }
            catch (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Không để số lượng bằng 0',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        this.getCart();
                    }
                });
            }
        }
    }
});