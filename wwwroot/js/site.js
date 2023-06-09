﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
  $("#borrarPago").click(function (event) {
    $("#FechaPago").val(null);
  });
});

$(document).ready(function () {
  $(".elSelect").select2();
});

function init() {
  $(".alert").alert();
}

$(document).ready(function () {
  // Setup - add a text input to each footer cell
  $(".tableplus thead tr")
    .clone(true)
    .addClass("filters")
    .appendTo(".tableplus thead");

  var table = $(".tableplus").DataTable({
    language: {
      lengthMenu: "Mostrar _MENU_ registros por página",
      zeroRecords: "Nada que mostrar",
      info: "Página _PAGE_ de _PAGES_",
      infoEmpty: "No hay nada",
      infoFiltered: "(filtered from _MAX_ total records)",
      search: "Buscar:",
      paginate: {
        previous: "Pagina previa",
        next: "Pagina siguiente",
      },
    },
    orderCellsTop: true,
    fixedHeader: true,
    initComplete: function () {
      var api = this.api();

      // For each column
      api
        .columns()
        .eq(0)
        .each(function (colIdx) {
          // Set the header cell to contain the input element
          var cell = $(".filters th").eq(
            $(api.column(colIdx).header()).index()
          );
          var title = $(cell).text();
          $(cell).html('<input type="text" placeholder="" />');

          // On every keypress in this input
          $(
            "input",
            $(".filters th").eq($(api.column(colIdx).header()).index())
          )
            .off("keyup change")
            .on("change", function (e) {
              // Get the search value
              $(this).attr("title", $(this).val());
              var regexr = "({search})"; //$(this).parents('th').find('select').val();

              var cursorPosition = this.selectionStart;
              // Search the column for that value
              api
                .column(colIdx)
                .search(
                  this.value != ""
                    ? regexr.replace("{search}", "(((" + this.value + ")))")
                    : "",
                  this.value != "",
                  this.value == ""
                )
                .draw();
            })
            .on("keyup", function (e) {
              e.stopPropagation();

              $(this).trigger("change");
              $(this)
                .focus()[0]
                .setSelectionRange(cursorPosition, cursorPosition);
            });
        });
    },
  });
});

$(document).ready(function () {
  $(".table").DataTable({
    language: {
      lengthMenu: "Mostrar _MENU_ registros por página",
      zeroRecords: "Nada que mostrar",
      info: "Página _PAGE_ de _PAGES_",
      infoEmpty: "No hay nada",
      infoFiltered: "(filtered from _MAX_ total records)",
      search: "Buscar:",
      paginate: {
        previous: "Pagina previa",
        next: "Pagina siguiente",
      },
    },
  });
});
