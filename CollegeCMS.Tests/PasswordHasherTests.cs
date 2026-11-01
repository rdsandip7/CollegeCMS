using BusinessLayer;

namespace CollegeCMS.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_verifies_with_the_same_password()
    {
        var hash = PasswordHasher.Hash("correct horse");

        Assert.True(PasswordHasher.Verify("correct horse", hash));
        Assert.False(PasswordHasher.Verify("wrong horse", hash));
    }

    [Fact]
    public void Hash_is_salted()
    {
        Assert.NotEqual(PasswordHasher.Hash("same"), PasswordHasher.Hash("same"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("plain-text-password")]
    [InlineData("v1.100000.not-base64.also-not")]
    public void Verify_rejects_malformed_hashes(string stored)
    {
        Assert.False(PasswordHasher.Verify("anything", stored));
    }
}
