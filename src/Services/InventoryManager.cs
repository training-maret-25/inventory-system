#region FuncName
public async Task<List<ModelName>> FuncName(IDbTransaction transaction, string? keyword, int offset, int limit)
{
  var p = db.Symbol();

  string query = $@"";

  query = QueryLimitOffset(query);

  var parameters = new
  {
    Keyword = $"%{keyword}%",
  };

  return await _command.GetRows<ModelName>(transaction, query, parameters);
}
#endregion