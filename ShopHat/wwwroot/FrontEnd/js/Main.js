var main_front = new Vue({
    el: '#main_front',
    data: {
        dataBrand: [],
        dataBrandNews: [],
        dataClb: [],
        dataSub: [],
        dataProducts: [],


        CountBrand: "",
        CountClb: "",
        CountProducts: "",
        CountBrandCus: 7,

        pageNumber: 1,
        pageSize: 1,
        selected: 'main',
        AboutMain: ''
    },
    created() {
        EventBus.$on('dataBrands', (value) => {
            this.dataBrand = value.slice(0, 7);
        });
        EventBus.$on('MainPage', (value) => {
            this.AboutMain = value;
        });
        //const savedSelection = localStorage.getItem('selectedSection');
        //if (savedSelection) {
        //    this.selected = savedSelection;
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
    watch: {
        CountProducts: function (newValue) {
            EventBus.$emit('countProductsChanged', newValue);
        },
        dataProducts(valuePr) {
            EventBus.$emit('dataProducts', valuePr);
        }
        
        
    },
    mounted() {

        $('#preloader').fadeIn();
        //this.dataBrand.slice(0, 7);

        //axios.get("/Home/GetAllBrands")
        //    .then((response) => {
        //        this.dataBrand = response.data.brands.slice(0, 7);
        //        this.dataBrandNews = response.data.brands;
        //        this.CountBrand = response.data.count;
        //        this.CountBrandCus = 7;

        //        return Promise.resolve();
        //    })
     
        axios.get("/Home/GetAllClb")
            .then((response) => {
                this.dataClb = response.data.clb;
                this.CountClb = response.data.count;

                return Promise.resolve();
            })
        axios.get("/Home/GetAllSubCategory")
            .then((response) => {
                this.dataSub = response.data.subCategories;
                return Promise.resolve();
            })
        axios.get(`/Home/GetAllProduct?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`)
            .then((response) => {
                this.dataProducts = response.data.products;
                this.CountProducts = response.data.count;
                return Promise.resolve();
            })
        $('#preloader').fadeOut();


    },
    methods: {
       
        selectSection(section) {
            this.selected = section;
            const expirationTime = new Date().getTime() + 3600 * 1000;
            localStorage.setItem('selectedSection', JSON.stringify({ section, expirationTime }));

            window.location.reload();
        },
        loadMoreProducts() {
            $('#preloader').fadeIn();

            this.pageNumber++; 
            axios.get(`/Home/GetAllProduct?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`)
                .then((response) => {
                    this.dataProducts = [...this.dataProducts, ...response.data.products];
                    $('#preloader').fadeOut();

                });
        }
    }


});