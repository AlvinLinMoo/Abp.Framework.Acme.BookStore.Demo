$(function () {
    var l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager(abp.appPath + 'Authors/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Authors/EditModal');

    var bookCreateModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');
    var bookEditModal = new abp.ModalManager(abp.appPath + 'Books/EditModal');

    var dataTable = $('#AuthorsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(acme.bookStore.authors.author.getList),
            columnDefs: [
                {
                    class: 'details-control',
                    orderable: false,
                    data: null,
                    defaultContent: ''
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('NewBook'),
                                    visible:
                                        abp.auth.isGranted('BookStore.Books.Create'),
                                    action: function (data) {
                                        bookCreateModal.open({ authorId: data.record.id });

                                        bookCreateModal.onResult(function () {
                                            // Fine BookListTable by AuthorId and reload
                                            var bookTable = $("#BooksTable" + data.record.id).DataTable();
                                            bookTable.ajax.reload();
                                        });
                                    }
                                },
                                {
                                    text: l('Edit'),
                                    visible:
                                        abp.auth.isGranted('BookStore.Authors.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible:
                                        abp.auth.isGranted('BookStore.Authors.Delete'),
                                    confirmMessage: function (data) {
                                        return l('AuthorDeletionConfirmationMessage',data.record.name);
                                    },
                                    action: function (data) {
                                        acme.bookStore.authors.author
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(
                                                    l('SuccessfullyDeleted')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                },
                                {
                                    text: l('Details'),
                                    visible: abp.auth.isGranted('BookStore.Books'), //CHECK for the PERMISSION
                                    action: function (data) {
                                        window.location.href = "/Authors/details?authorId=" + data.record.id;
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('Name'),
                    data: "name",
                    render: function (data, type, row, meta) {
                        // Redirect to Authors Details page
                        return `<a href='/Authors/details?authorId=${row.id}'>${data}</a>`;
                        //return `<a href='/Books?authorId=${row.id}'>${data}</a>`;
                        //return data;
                    }
                },
                {
                    title: l('BirthDate'),
                    data: "birthDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString();
                    }
                }
            ]
        })
    );

    // Array to track the ids of the details displayed rows
    var detailRows = [];

    $('#AuthorsTable tbody').on('click', 'tr td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);
        var idx = detailRows.indexOf(tr.attr('id'));

        if (row.child.isShown()) {
            tr.removeClass('details');
            row.child.hide();

            // Remove from the 'open' array
            detailRows.splice(idx, 1);
        } else {
            tr.addClass('details');
            row.child(format(row.data())).show();

            // Add to the 'open' array
            if (idx === -1) {
                detailRows.push(tr.attr('id'));
            }
        }
    });

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewAuthorButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    function format(d) {
        console.log(d);

        // d.AuthorId is the id of the clicked row
        // Generate DataTable for Book List Table by AuthorId
        var booksTableTemplate = $("#BooksTableTemplate").clone();
        var booksTableElement = $(booksTableTemplate.html());
        var booksTable = booksTableElement.find("#BooksTable");
        booksTable.attr("id", "BooksTable" + d.id);
        booksTable.addClass("childTable");

        //var booksTable = $(`<table class="table table-bordered table-hover table-striped" id="BookListTable${d.id}"></table>`);

        booksTable.DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(
                    acme.bookStore.books.book.getListByAuthorId, function () { return { authorId: d.id }; }
                ),
                columnDefs: [
                    {
                        title: l('Actions'),
                        rowAction: {
                            items:
                                [
                                    {
                                        text: l('Edit'),
                                        visible: abp.auth.isGranted('BookStore.Books.Edit'), //CHECK for the PERMISSION
                                        action: function (data) {
                                            bookEditModal.open({ id: data.record.id });
                                            bookEditModal.onResult(function () {
                                                // Fine BookListTable by AuthorId and reload
                                                var bookTable = $("#BooksTable" + d.id).DataTable();
                                                bookTable.ajax.reload();  
                                            });
                                        }
                                    },
                                    {
                                        text: l('Delete'),
                                        visible: abp.auth.isGranted('BookStore.Books.Delete'), //CHECK for the PERMISSION
                                        confirmMessage: function (data) {
                                            return l(
                                                'BookDeletionConfirmationMessage', data.record.name);
                                        },
                                        action: function (data) {
                                            acme.bookStore.books.book
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.notify.info(l('SuccessfullyDeleted'));
                                                    // Fine BookListTable by AuthorId and reload
                                                    var bookTable = $("#BooksTable" + d.id).DataTable();
                                                    bookTable.ajax.reload();    
                                                });
                                        }
                                    }
                                ]
                        }
                    },
                    {
                        title: l('Name'),
                        data: "name"
                    },
                    // ADDED the NEW AUTHOR NAME COLUMN
                    {
                        title: l('Author'),
                        data: "authorName"
                    },
                    {
                        title: l('Type'),
                        data: "type",
                        render: function (data) {
                            return l('Enum:BookType.' + data);
                        }
                    },
                    {
                        title: l('PublishDate'),
                        data: "publishDate",
                        render: function (data) {
                            return luxon
                                .DateTime
                                .fromISO(data, {
                                    locale: abp.localization.currentCulture.name
                                }).toLocaleString();
                        }
                    },
                    {
                        title: l('Price'),
                        data: "price"
                    },
                    {
                        title: l('CreationTime'), data: "creationTime",
                        render: function (data) {
                            return luxon
                                .DateTime
                                .fromISO(data, {
                                    locale: abp.localization.currentCulture.name
                                }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                        }
                    }
                ]
            })
        );

        return booksTable;

        //return (
        //    'Full name: ' +
        //    d.name +
        //    '<br>' +
        //    'birthDate: ' +
        //    d.birthDate +
        //    '<br>' +
        //    'The child row can contain any data you wish, including links, images, inner tables etc.'
        //);
    }
});
