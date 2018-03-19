
using System;

public class RandomString
{
	const string _lower_str = "abcdefghijklmnopqrstuvwxyz0123456789";
	const string _upper_str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	/**
	 * 生成随机字符串
	 * @param {number} len 字符串长度，默认16字节
	 * @param {boolean} all_char 若为true则包含大小写字母，若为false只包含小写字母。默认为true
	 * @return {string}
	 * */
	public static string Generate(int len = 16, bool all_char = true)
	{
		string str = _lower_str;
		if (all_char)
			str = _upper_str;

		string res = "";
		for (int i = 0; i < len; ++i)
		{
            Random r3 = new Random(str.Length - 1);
            int rv = r3.Next();

			// 求余结果为0 - (n-1)
			int idx = rv % str.Length;
			res = res + str.Substring(idx, 1);
		}

		return res;
	}

}