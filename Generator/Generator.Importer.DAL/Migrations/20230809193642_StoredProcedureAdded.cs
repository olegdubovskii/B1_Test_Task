using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Generator.Importer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedureAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcSql = @"CREATE PROCEDURE [dbo].[GetIntSummAndDoubleMedian]
                @Sum BIGINT OUTPUT,
                @Median FLOAT OUTPUT
            AS
            BEGIN
                SELECT @Sum = SUM(CAST(RandomInt AS BIGINT)) FROM FileString;

                DECLARE @Count BIGINT = (SELECT COUNT(*) FROM FileString);

                SELECT @Median = AVG(1.0 * RandomDouble) FROM
                    (SELECT RandomDouble FROM FileString
                    ORDER BY RandomDouble
                    OFFSET (@Count - 1) / 2 ROWS
                    FETCH NEXT 1 + (1 - @Count % 2) ROWS ONLY) as Median;

            END;";
            migrationBuilder.Sql(createProcSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcSql = "DROP PROC [dbo].[GetIntSummAndDoubleMedian]";
            migrationBuilder.Sql(dropProcSql);
        }
    }
}
