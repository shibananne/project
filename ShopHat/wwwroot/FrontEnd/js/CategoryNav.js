var nav_header_vue = new Vue({
    el: '#nav_header_vue',
    data: {
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
        
    },
    created: function () {
      
        EventBus.$on('dataBrands', (value) => {
            this.dataBrandNews = value;
        });
        EventBus.$on('isNavClick', (value) => {
            this.isNav = value;
        });
     
    },
    mounted() {
        $('#preloader').fadeIn();

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

        $('#preloader').fadeOut();

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
        } ,
        categoryDetailsTeam(val) {
            location.href = `/Teams/${val.slug}`

        }
    }
});