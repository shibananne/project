var check_vue = new Vue({

    el: '#check_vue',
    data: {
        dataCate: [],
        host: 'https://provinces.open-api.vn/api/',
        cities: [],
        districts: [],
        wards: [],
        selectedCity: '',
        selectedDistrict: '',
        selectedWard: '',
        result: "",
        detailsAddress: "",

        CartAdd: [],
        orderId: 0,
        FullName: "",
        Email: "",
        Phone: "",
        isFocused: false,
        isCop: false,
        isEr: false,
        Cop: "",
        ValueCop: null,
    },
    computed: {
        totalPrices() {
            let totalPrice = 0;
            if (this.CartAdd && this.CartAdd.length > 0) {
                this.CartAdd.forEach(item => {
                    totalPrice += item.totalPrices;
                });
            }
            if (this.ValueCop != null) {
                return Math.round(totalPrice * ((100 - this.ValueCop) / 100));
            }
            return Math.round(totalPrice);
        },
        discountedPrice() {
            if (this.ValueCop != null) {
                return Math.round(this.totalPrices * ((100 - this.ValueCop) / 100));
            }
            return 0;
        }
    },
    created: function () {
        EventBus.$on('dataCart', (value) => {
            this.CartAdd = value;
        });
        EventBus.$on('orderId', (value) => {
            this.orderId = value;
        });
    },
    mounted() {
        $('#preloader').fadeIn();

        axios.get(`/Home/GetAllSubCategoryWithId/1`)
            .then((response) => {
                this.dataCate = response.data.subCategories;
                return Promise.resolve();
            })
        this.callAPI(this.host + '?depth=1');
       
        $('#preloader').fadeOut();
    },
   
    watch: {
       
    },
    methods: {

        async applyCoupon() {
            $('#preloader').fadeIn();

            const formData = new FormData();

            formData.append('Cop', this.Cop);

            await axios.post('/Checkout/ConfirmCoupon', formData,
                {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }).then((res) => {
                    if (res.data.code == 200) {
                        this.isEr = false;
                        this.ValueCop = res.data.coupon.contentCop;
                        console.log(this.ValueCop);
                    }
                    else if (res.data.code == 203) {
                        this.isEr = true;
                        this.ValueCop = null;

                    }
                  
                })
            $('#preloader').fadeOut();
        },
        openCop() {
            this.isCop = true;
        },
        onFocus() {
            this.isFocused = true;
        },
        onBlur(event) {
            if (event.target.value === "") {
                this.isFocused = false;
            }
        },
        formatPrice(value) {
            if (!value) return '0 VND';

            return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") + ' VND';
        },
        callAPI(api) {
            fetch(api)
                .then(response => {
                    return response.json()
                })
                .then(data => {
                    this.cities = data;
                })
                .catch(error => {
                    console.error(error);
                });
        },
        getDistricts() {
            if (this.selectedCity) {
                const api = this.host + 'p/' + this.selectedCity + '?depth=2';
                fetch(api)
                    .then(response => {
                        return response.json()

                    }).then((data) => {
                        this.districts = data.districts;
                        this.selectedDistrict = '';
                        this.wards = [];
                        this.selectedWard = '';
                        this.result = '';
                    })
                    .catch(error => {
                        console.error(error);
                    });
            } else {
                this.districts = [];
                this.wards = [];
                this.result = '';
            }
        },
        getWards() {
            if (this.selectedDistrict) {
                const api = this.host + 'd/' + this.selectedDistrict + '?depth=2';
                fetch(api)
                    .then(response => {
                        return response.json()
                    }).then(data => {
                        this.wards = data.wards;
                        this.selectedWard = '';
                        this.result = '';
                    })
                    .catch(error => {
                        console.error(error);
                    });
            } else {
                this.wards = [];
                this.result = '';
            }
        },
        printResult() {
            if (this.selectedCity && this.selectedDistrict && this.selectedWard) {
                const city = this.cities.find(c => c.code === this.selectedCity);

                const district = this.districts.find(d => d.code === this.selectedDistrict);
                const ward = this.wards.find(w => w.code === this.selectedWard);

                this.result = `Thành phố: ${city.name} , Quận/Huyện: ${district.name} , Phường/Xã: ${ward.name}`;
                if (this.detailsAddress && this.detailsAddress.trim() !== '') {
                    this.result += `, Chi tiết: ${this.detailsAddress}`;
                }
            } else {
                this.result = '';
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
                $('#preloader').fadeIn();

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
                $('#preloader').fadeOut();

                header_vue.getCart();

            }
            catch (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: 'Không để số lượng bằng 0',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        header_vue.getCart();
                    }
                });
            }
        },
        async deleteCart(product) {
            try {
                $('#preloader').fadeIn();

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
                $('#preloader').fadeOut();

                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        header_vue.getCart();
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
        async ConfirmCart() {
            try {
                $('#preloader').fadeIn();
                const firstName = $("#firstName").val();
                const lastName = $("#lastName").val();

                this.FullName = firstName + " " + lastName;

                if (firstName == "" || lastName == "") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi ',
                        text: 'Không để trống họ tên',
                        confirmButtonText: 'OK'
                    });
                    $('#preloader').fadeOut();

                    return;
                }
                if (this.Phone == "" || !/^0\d{9}$/.test(this.Phone)) {
                    let errorMessage = this.Phone == "" ? 'Không để trống số điện thoại' : 'Số điện thoại phải bắt đầu bằng số 0';
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi',
                        text: errorMessage,
                        confirmButtonText: 'OK'
                    });
                    $('#preloader').fadeOut();
                    return;
                }
                if (this.selectedCity == '' && this.selectedWard == '' && this.selectedDistrict == '' && this.detailsAddress == '' ) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi ',
                        text: 'Không để trống địa chỉ',
                        confirmButtonText: 'OK'
                    });
                    $('#preloader').fadeOut();

                    return;
                }
                const formData = new FormData();
              
                formData.append('OrderId', this.orderId);
                formData.append('FullName', this.FullName);
                formData.append('Email', this.Email);
                formData.append('City', this.selectedCity);
                formData.append('District', this.selectedDistrict);
                formData.append('Ward', this.selectedWard);
                formData.append('DetailsAddress', this.detailsAddress);
                formData.append('FullAddress', this.result);
                formData.append('Phone', this.Phone);


                await axios.post('/Checkout/ConfirmCart', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                $('#preloader').fadeOut();

                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: 'Đã lưu thành công',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        header_vue.getCart();
                        window.location.href = '/Checkout/ThankYou';
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
                        header_vue.getCart();


                    }
                });
            }
        }
    },

});