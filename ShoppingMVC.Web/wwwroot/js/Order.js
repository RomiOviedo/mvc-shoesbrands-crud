var table;
$(function () {                                              // espera q se cargue el DOM(DOCUMENT OBJECT MODEL)Representa HTML en memoria, lo convierte en arbol ejecutable en JSON(JAVASCRIPT OBJECT NOTATION)
    loadDataTable();                                        // se ejecuta cuando se carga el DOM
});
(
    function loadDataTable(){                                //le indico q se llene la tabla con los datos q traigo del controller

        dataTable = new DataTable('#orderTable',             //es el id table id="orderTable" del index  | el # es como se inicia la tabla
            {
                "ajax":                                     //ajax= asynchronous javascript and XML=> PIDE DATOS AL SERVIDOR SIN RECARGAR LA PAGINA
                {
                    "url": "/Controller/Order/GetAll"   // me redirige al jsonResult GetAll del controlles, me devuelve un object json
                },
                "colums": [                                  // creo las colums dentro de un array porque son varias
                    {
                        "data": "orderHeaderId"
                    },
                    {
                        "data": "orderDate",
                        "render": function (data) {
                            return moment(data).format('DD/MM/YYYY');

                        }

                    },
                    {
                        "data": "shippingDate",
                        "render": function (data) {
                            return numeral(data).format('$0,0.00')
                        }
                    },
                    {
                        "data": "orderHeaderId",
                        "render": function (data)
                        {
                            return  `
                                < a class="btn btn-info" href="/Views/Order/Details?id=${data}">
                                    <i class="bi bi-card-list"></i> & nbsp;
                            Details
                            </a > 
                            ` // `  backtick = alt+96
                        }
                    }

                ]

            });
    }